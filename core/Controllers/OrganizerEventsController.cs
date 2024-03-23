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

[ApiController]
[Authorize(Policy = AuthPolicies.OrganizerIsActive)]
[Route("api/organizer/events")]
public class OrganizerEventsController(ILogger<OrganizerEventsController> logger, IEventService eventService, IUserService userService) : ControllerBase
{
    [HttpGet]
    public IActionResult MyEvents(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] IEnumerable<string>? activityAreas,
        [FromQuery] IEnumerable<Guid>? tags,
        [FromQuery] PaginationRequest pagination,
        [FromQuery] State state = State.All
        )
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);

        logger.LogInformation("Getting events");
        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(startDate, endDate, activityAreas, tags, userId, state, ignorePublicationDate: true);
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
