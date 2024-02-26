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
            new StatusChangeModel
            {
                Title = "Changement de status",
                Salutation = "Bonjour ApplETS,",
                StatusHeaderText = "La publication « Compétition de développement mobile » a été",
                StatusNameText = "refusée",
                StatusRefusalHeader = "Raison du refus :",
                StatusRefusalReason = "La publication ne respecte pas les règles de publication",
                ButtonSeePublicationText = "Voir la publication",
            },
            emails.EmailsUtils.StatusChangeTemplate);

        return Ok();
    }
}
#endif
