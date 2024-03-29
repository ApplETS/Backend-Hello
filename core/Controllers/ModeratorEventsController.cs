using api.core.Data;
using api.core.Data.Enums;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicies.IsModerator)]
[Route("api/moderator/events")]
public class ModeratorEventsController(ILogger<ModeratorEventsController> logger, IEventService eventService, IReportService reportService) : ControllerBase
{
    [HttpPatch("{id}/state")]
    public IActionResult UpdateEventState(Guid id, [FromQuery] State newState, [FromQuery] string? reason)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return eventService.UpdateEventState(userId, id, newState, reason) ? Ok() : BadRequest();
    }

    [HttpGet]
    public ActionResult<IEnumerable<EventResponseDTO>> GetEventsModerator(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] IEnumerable<string>? activityAreas,
        [FromQuery] IEnumerable<Guid>? tags,
        [FromQuery] PaginationRequest pagination,
        [FromQuery] State state = State.All
        )
    {
        if (state.HasFlag(State.Draft))
            state &= ~State.Draft; // Exclude Draft data for moderators view
        
        logger.LogInformation("Getting events");

        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(startDate, endDate, activityAreas, tags, null, state, ignorePublicationDate: true);
        var totalRecords = events.Count();
        var paginatedRes = events
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
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
