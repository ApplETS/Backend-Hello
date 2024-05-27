using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.Data;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

/// <summary>
/// This API is used to manage draft events, which are events that are not yet saved and visible
/// for moderation. Only the organizer can see and manage his own draft events. Draft events are
/// not visible to the public. Since the EventsController is already used to fetch all events of 
/// any types, it is not possible to fetch draft events from there. This controller is used to only
/// create/update draft events.
/// 
/// Under the hood, this controller uses the IDraftEventService to manage the data by passing the
/// userId found in your JWT token. It is only accessible to organizers that are active (not 
/// disabled by moderator). 
/// </summary>
/// <param name="logger">
/// A logger provider which allows logging valuable info,warn,error to the console or a
/// chosen provider (e.g. file, database, etc.)
/// </param>
/// <param name="draftService">The service to allows creating and managing draft</param>
[ApiController]
[Authorize(Policy = AuthPolicies.OrganizerIsActive)]
[Route("api/organizer-drafts")] // TODO : Change route to /api/me/drafts
public class DraftEventsController(ILogger<DraftEventsController> logger, IDraftEventService draftService) : ControllerBase
{
    /// <summary>
    /// Add a draft event to the database. This event will be saved as a draft and will not be visible
    /// for others.
    /// </summary>
    /// <param name="draftEvent">The draftEvent request containing mostly the same field as for regular event creation</param>
    /// <returns>The draft event response freshly created.</returns>
    [HttpPost]
    public IActionResult AddDraft([FromForm] DraftEventRequestDTO draftEvent)
    {
        logger.LogInformation($"Adding new draft");

        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var evnt = draftService.AddDraftEvent(userId, draftEvent);

        return new OkObjectResult(
            new Response<EventResponseDTO>
            {
                Data = evnt,
            });
    }

    /// <summary>
    /// Update a draft event in the database. This events needs to be a draft, if not it wil be 
    /// converted as such and will not be visible anymore.
    /// </summary>
    /// <param name="id">The draft event id to modify</param>
    /// <param name="draftEvent">the value to modify, make sure to pass all required value, since thery will override the previous values</param>
    /// <returns></returns>
    [HttpPatch("{id}")] // TODO: Change this to a HttpPut instead
    public IActionResult UpdateDraft(Guid id, [FromForm] DraftEventRequestDTO draftEvent)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return draftService.UpdateDraftEvent(userId, id, draftEvent) ? Ok() : BadRequest();
    }
}
