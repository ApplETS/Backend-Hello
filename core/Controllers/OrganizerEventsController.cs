using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.Data;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using api.core.Data.Requests;
using api.core.Data.Enums;

namespace api.core.Controllers;

/// <summary>
/// A controller for handling events related to the organizer. This controller is used by 
/// the organizer to manage their events. They can create, update, delete and get their events.
/// </summary>
/// <param name="logger">A logger provider to log informations in the controller</param>
/// <param name="eventService">A service to manage events</param>
[ApiController]
[Authorize(Policy = AuthPolicies.OrganizerIsActive)]
[Route("api/organizer-events")]
public class OrganizerEventsController(ILogger<OrganizerEventsController> logger, IEventService eventService) : ControllerBase
{
    /// <summary>
    /// Fetch events for the currently connected organizer
    /// </summary>
    /// <param name="startDate">if null, will get every event until the first element</param>
    /// <param name="endDate">if null, will get every element unitl infinity</param>
    /// <param name="title">Filter by title</param>
    /// <param name="activityAreas">Filter by a list of OR applicable activityAreas</param>
    /// <param name="tags">Filter by a list of OR applicable tags</param>
    /// <param name="state">Filter by state or multiple by using the </param>
    /// <param name="ordering">Sort and take only the necessary page</param>
    /// <param name="pagination">Sort and take only the necessary page</param>
    /// <returns>events filtered and sorted</returns>
    [HttpGet]
    public IActionResult MyEvents(
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
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);

        logger.LogInformation("Getting events");
        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(
            startDate, endDate,
            activityAreas, tags,
            userId, title, state,
            ordering.OrderBy ?? "EventStartDate", ordering.Descending,
            ignorePublicationDate: true);
        var totalRecords = events.Count();
        var paginatedRes = events
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var response = PaginationHelper.CreatePaginatedReponse(paginatedRes, validFilter, totalRecords);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult AddEvent([FromForm] EventCreationRequestDTO dto)
    {
        logger.LogInformation($"Adding new event");

        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var evnt = eventService.AddEvent(userId, dto);

        return new OkObjectResult(
            new Response<EventResponseDTO>
            {
                Data = evnt,
            });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEvent(Guid id)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var isDeleted = eventService.DeleteEvent(userId, id);
        return isDeleted ? Ok() : BadRequest();
    }

    [HttpPatch("{id}")]
    public IActionResult UpdateEvent(Guid id, [FromForm] EventUpdateRequestDTO dto)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return eventService.UpdateEvent(userId, id, dto) ? Ok() : BadRequest();
    }
}
