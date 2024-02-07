
using api.emails.Models;
using api.emails.Services;
using api.emails.Services.Abstractions;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using System.Net;

namespace api.emails;

public static class SetupMailService
{
    public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        // BUILD configuration for settings
        var settings = new EmailSettings
        {
            Host = configuration.GetValue<string>("EMAIL_SERVER") ?? "",
            Port = configuration.GetValue<int>("EMAIL_PORT"),
            Username = configuration.GetValue<string>("EMAIL_USERNAME") ?? "",
            Password = configuration.GetValue<string>("EMAIL_PASSWORD") ?? ""
        };

        // BUILD add services to DI
        Email.DefaultRenderer = new CustomRazorRenderer();
        Email.DefaultSender = new SmtpSender(new SmtpClient(settings.Host, settings.Port)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(settings.Username, settings.Password)
        });

        services.AddTransient<IEmailService, EmailService>();
    }
}
