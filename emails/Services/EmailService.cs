using api.emails.Services.Abstractions;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;

namespace api.emails.Services;
public class EmailService(ILogger<EmailService> logger, IFluentEmail fluentEmail) : IEmailService
{
    public async Task<SendResponse?> SendEmailAsync<T>(string emailTo, string subject, T model, string templateName)
    {
        logger.LogInformation($"Sending email to {emailTo} with subject {subject} and template {templateName}");

#if DEBUG
        emailTo = "samuel.montambault.1@ens.etsmtl.ca";
#endif

        return await fluentEmail
            .To(emailTo)
            .Subject(subject)
            .UsingTemplateFromEmbedded(EmailsUtils.TemplateEmbeddedResourceNamespace + templateName, model, GetType().Assembly)
            .SendAsync();
    }

}
