using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.Data;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicies.OrganizerIsActive)]
[Route("api/organizer-drafts")]
public class DraftEventsController(ILogger<DraftEventsController> logger, IDraftEventService draftService) : ControllerBase
{
    [HttpPost]
    public IActionResult AddDraft([FromForm] DraftEventRequestDTO dto)
    {
        logger.LogInformation($"Adding new draft");

        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var evnt = draftService.AddDraftEvent(userId, dto);

        return new OkObjectResult(
            new Response<EventResponseDTO>
            {
                Data = evnt,
            });
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateDraft(Guid id, [FromForm] DraftEventRequestDTO dto)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return draftService.UpdateDraftEvent(userId, id, dto) ? Ok() : BadRequest();
    }
}
