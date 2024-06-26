using api.core.Data.Enums;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

/// <summary>
/// Controller for handling events for moderators. It can only be accessible from moderators.
/// This allows moderators to view all events On Hold (not yet published), and approve or reject
/// the events.
/// 
/// Under the hood, the controller uses the EventService to fetch the events and update their state.
/// </summary>
/// <param name="logger">A logger provider to log informations in the controller</param>
/// <param name="eventService">Used to fetch events</param>
/// <param name="userService">Used to fetch the avatar url of the organiser for each event fetched</param>
[ApiController]
[Authorize(Policy = AuthPolicies.IsModerator)]
[Route("api/moderator-events")]
public class ModeratorEventsController(
    ILogger<ModeratorEventsController> logger,
    IEventService eventService,
    IUserService userService) : ControllerBase
{
    /// <summary>
    /// Update the state of an event. This is used for a moderator that needs to 
    /// approve or reject an event.
    /// </summary>
    /// <param name="id">The id of the event to update its state</param>
    /// <param name="newState">The new state </param>
    /// <param name="reason">
    /// A reason for the status change, generally used when rejecting 
    /// a publication to notify the user of a specific reason it was rejected.
    /// </param>
    /// <returns>whether the state was updated or not</returns>
    [HttpPatch("{id}/state")]
    public IActionResult UpdateEventState(Guid id, [FromQuery] State newState, [FromQuery] string? reason)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return eventService.UpdateEventState(userId, id, newState, reason) ? Ok() : BadRequest();
    }

    /// <summary>
    /// Get all events from moderators view. This will return all events that are not yet published.
    /// </summary>
    /// <param name="startDate">if null, will get every event until the first element</param>
    /// <param name="endDate">if null, will get every element unitl infinity</param>
    /// <param name="title">Filter by title</param>
    /// <param name="activityAreas">Filter by a list of OR applicable activityAreas</param>
    /// <param name="tags">Filter by a list of OR applicable tags</param>
    /// <param name="ordering"></param>
    /// <param name="state">Filter by state, you can fetch only </param>
    /// <param name="pagination">Sort and take only the necessary page</param>
    /// <returns>events filtered and sorted</returns>
    /// <returns>All events matching any filters passed in params</returns>
    [HttpGet]
    public ActionResult<IEnumerable<EventModeratorResponseDTO>> GetEventsModerator(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] IEnumerable<Guid>? activityAreas,
        [FromQuery] IEnumerable<Guid>? tags,
        [FromQuery] string? title,
        [FromQuery] OrderingRequest ordering,
        [FromQuery] PaginationRequest pagination,
        [FromQuery] State state = State.All
        )
    {
        if (state.HasFlag(State.Draft))
            state &= ~State.Draft; // Exclude Draft data for moderators view
        
        logger.LogInformation("Getting events");

        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(
            startDate, endDate,
            activityAreas, tags,
            null, title, state,
            ordering.OrderBy ?? "EventStartDate", ordering.Descending,
            ignorePublicationDate: true);

        var totalRecords = events.Count();
        var paginatedRes = events
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList()
            .Select((e) =>
            {
                var url = userService.GetUserAvatarUrl(e.Organizer.Id);
                e.Organizer.AvatarUrl = url;
                return e;
            })
            .ToList();

        var response = PaginationHelper.CreatePaginatedReponse(paginatedRes, validFilter, totalRecords);

        return Ok(response);
    }
}
