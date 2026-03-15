using MailKit.Net.Smtp;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SurveyBasket.Settings;

namespace SurveyBasket.Health;

public class MailProviderHealthCheck(IOptions<MailSettings> options) : IHealthCheck
{
    private readonly MailSettings _options = options.Value;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            smtp.Authenticate(_options.Mail, _options.Password, cancellationToken);
            return await Task.FromResult(HealthCheckResult.Healthy("Mail provider is healthy"));
        }
        catch(Exception exeption)
        {
            return await Task.FromResult(HealthCheckResult.Unhealthy(exeption.Message));
        }
    }
}
