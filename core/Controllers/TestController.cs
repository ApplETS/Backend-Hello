using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.services.abstractions;
using api.emails.Models;
using api.emails.Services.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

[ApiController]
[Route("api/test")]
public class TestController(IEmailService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrganizerResponseDTO>> Create()
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
