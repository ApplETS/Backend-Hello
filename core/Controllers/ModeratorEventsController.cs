using api.core.Data;
using api.core.Data.Enums;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;
using api.core.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicies.IsModerator)]
[Route("api/moderator/events")]
public class ModeratorEventsController(ILogger<ModeratorEventsController> logger, IEventService eventService, IReportService reportService, IUserService userService) : ControllerBase
{
    [HttpPatch("{id}/state")]
    public IActionResult UpdateEventState(Guid id, [FromQuery] State newState, [FromQuery] string? reason)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return eventService.UpdateEventState(userId, id, newState, reason) ? Ok() : BadRequest();
    }

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

    [HttpGet("reports")]
    public ActionResult<IEnumerable<ReportResponseDTO>> GetEventsReports()
    {
        var reports = reportService.GetReports();

        return Ok(new Response<IEnumerable<ReportResponseDTO>>
        {
            Data = reports,
        });
    }
}
