using FluentEmail.Core.Models;
namespace api.emails.Services.Abstractions;

public interface IEmailService
{
    /// <summary>
    /// Send an email with a given subject and razor template
    /// </summary>
    /// <typeparam name="T">The model type you want to send. Make sure to use the related email model located in api.emails.Models.</typeparam>
    /// <param name="emailTo">The recipient of the email you can pass multiple split by <code>;</code></param>
    /// <param name="subject">The subject of the email sent out</param>
    /// <param name="model">The model of the T type pass in param</param>
    /// <param name="templateName">The razor filename to access, You can use EmailsUtils.YOUR_TEMPLATE</param>
    /// <returns>The Send Response object provided by FluentEmail</returns>
    Task<SendResponse> SendEmailAsync<T>(string emailTo, string subject, T model, string templateName);
}
