
using api.emails.Models;
using api.emails.Services;
using api.emails.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace api.emails;

public static class SetupMailService
{
    public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        // BUILD configuration for settings
        var settings = new EmailSettings();
        configuration.GetSection("EmailSettings").Bind(settings);
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        // BUILD add services to DI
        services.AddFluentEmail(settings.From)
            .AddRazorRenderer(typeof(Assembly))
            .AddSmtpSender(settings.Host,
                settings.Port,
                settings.Username,
                settings.Password);

        services.AddTransient<IEmailService, EmailService>();
    }
}
