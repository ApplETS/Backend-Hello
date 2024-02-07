#if DEBUG
using api.core.Data.Responses;
using api.emails.Models;
using api.emails.Services.Abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

[ApiController]
[Route("api/mail")]
public class MailTestController(IEmailService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var result = await service.SendEmailAsync(
            "test recipient",
            "test subject",
            new HelloWorldModel
            {
                Title = "Hello World",
                Name = "Test name"
            },
            emails.EmailsUtils.ComplexHelloWorldTemplate);

        return Ok();
    }
}
#endif
