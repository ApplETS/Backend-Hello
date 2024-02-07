using api.emails.Models;
using api.emails.Services.Abstractions;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace api.emails.Services;
public class EmailService: IEmailService
{
    private readonly ILogger<EmailService> logger;
    private readonly EmailSettings settings;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        this.logger = logger;

        settings = new EmailSettings
        {
            From = configuration.GetValue<string>("EMAIL_FROM") ?? "",
            ToWhenDebugging = configuration.GetValue<string>("EMAIL_TO_IF_DEBUG") ?? ""
        };
    }

    public async Task<SendResponse?> SendEmailAsync<T>(string emailTo, string subject, T model, string templateName)
    {
        logger.LogInformation($"Sending email to {emailTo} with subject {subject} and template {templateName}");

#if DEBUG
        emailTo = settings.ToWhenDebugging;
#endif

        var email = Email
            .From(settings.From)
            .To(emailTo)
            .Subject(subject)
            .UsingTemplateFromEmbedded(EmailsUtils.TemplateEmbeddedResourceNamespace + templateName, model, GetType().Assembly);
        
        return await email.SendAsync();
    }

}
