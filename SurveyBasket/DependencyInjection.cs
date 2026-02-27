
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Authentication;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;
using SurveyBasket.Settings;
using System.Text;

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
            .AddSwaggerServices()
            .AddMapsterConfig().
            AddFluentValidationConfig();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IQuestionService, QuestionService>();


        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddBackgroundJobsConfig(configuration);

        services.AddHttpContextAccessor();

        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        return services;
    }
    private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(); 
        return services;
    }   
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

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

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
}
