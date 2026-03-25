using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Abstractions.Consts;
using SurveyBasket.Authentication;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Errors;
using SurveyBasket.Health;
using SurveyBasket.OpenApiTransformers;
using SurveyBasket.Persistence;
using SurveyBasket.Settings;
using System.Text;
using System.Threading.RateLimiting;

namespace SurveyBasket;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddControllers();
        services.AddHybridCache();


        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

        services.AddCors(options =>
              options.AddDefaultPolicy(builder => 
                  builder
                          .WithOrigins(allowedOrigins!)
                          .AllowAnyMethod()
                          .AllowAnyHeader()) 
        );

        services.AddAuthConfig(configuration);

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string \"DefaultConnection\" not found.");
        services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString));

        services
            //.AddSwaggerServices()
            .AddMapsterConfig().
            AddFluentValidationConfig();


        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IQuestionService, QuestionService>();


        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddBackgroundJobsConfig(configuration);
        services.AddHttpContextAccessor();

        services.AddOptions<MailSettings>()
            .BindConfiguration(nameof(MailSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHealthChecks()
            .AddSqlServer(name: "database", connectionString: connectionString)
            .AddHangfire(options => { options.MinimumAvailableServers = 1; })
            .AddCheck<MailProviderHealthCheck>(name : "Mail Provider");

        services.AddRateLimitConfig();
        services.AddApiVersioning(options =>
        {
            //options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
        }).AddApiExplorer(options =>
        {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
        });

        services.AddEndpointsApiExplorer()
            .AddOpenApiServices();

        return services;
        
    }
    private static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var apiVersionDescriptionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach(var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            services.AddOpenApi(description.GroupName,options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                options.AddDocumentTransformer(new ApiVersioningTransformer(description));
            });
        }

        return services;
    }

    //private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    //{
    //    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    //    services.AddSwaggerGen(); 
    //    return services;
    //}   
    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        // add mappster
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        return services;
    } 
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;

    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddSingleton<IJwtProvider,JwtProvider>();

        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;     // to set the default to bearer
        }).AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience
            };
        });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
            //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //options.Lockout.MaxFailedAccessAttempts = 5;
        });
        return services;

    }
    private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Hangfire services.
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;    
    }
    private static IServiceCollection AddRateLimitConfig(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.AddPolicy(RateLimitingConsts.ipLimiter, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    Window = TimeSpan.FromSeconds(20),
                    PermitLimit = 2
                }
                ));

            rateLimiterOptions.AddPolicy(RateLimitingConsts.userLimiter, httpContext =>
               RateLimitPartition.GetFixedWindowLimiter(
               partitionKey: httpContext.User.GetUserId(),
               factory: _ => new FixedWindowRateLimiterOptions
               {
                   Window = TimeSpan.FromSeconds(20),
                   PermitLimit = 2
               }
               ));


            rateLimiterOptions.AddConcurrencyLimiter(RateLimitingConsts.concurrency, options =>
            {
                options.PermitLimit = 1000;
                options.QueueLimit = 100;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            //rateLimiterOptions.AddTokenBucketLimiter("tokenBucket", options =>
            //{
            //    options.TokenLimit = 10;
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    options.QueueLimit = 1;
            //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(60);
            //    options.TokensPerPeriod = 2;
            //    options.AutoReplenishment = true;
            //});
            //rateLimiterOptions.AddFixedWindowLimiter("fixed", rateLimiterOptions =>
            //{
            //    rateLimiterOptions.PermitLimit = 100;
            //    rateLimiterOptions.Window = TimeSpan.FromMinutes(1);
            //    rateLimiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    rateLimiterOptions.QueueLimit = 0;

            //});
            //rateLimiterOptions.AddSlidingWindowLimiter("sliding", rateLimiterOptions =>
            //{
            //    rateLimiterOptions.PermitLimit = 2;
            //    rateLimiterOptions.Window = TimeSpan.FromSeconds(20);
            //    rateLimiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    rateLimiterOptions.QueueLimit = 0;
            //    rateLimiterOptions.SegmentsPerWindow = 6;
            //});

        });
        return services;
    }
}
