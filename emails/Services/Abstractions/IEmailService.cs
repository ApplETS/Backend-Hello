using FluentEmail.Core.Models;
namespace api.emails.Services.Abstractions;

public interface IEmailService
{
    /// <summary>
    /// Send an email with a given subject and razor template
    /// </summary>
    /// <param name="emailTo">The recipient of the email you can pass multiple split by <code>;</code></param>
    /// <param name="subject">The subject of the email sent out</param>
    /// <param name="templateName">The razor filename to access, You can use EmailsUtils.YOUR_TEMPLATE</param>
    /// <!-- T is the model to be used in the template -->
    Task<SendResponse> SendEmailAsync<T>(string emailTo, string subject, T model, string templateName);
}
