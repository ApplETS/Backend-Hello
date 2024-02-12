using api.core.Data;
using api.core.Data.Entities;
using api.core.Data.Exceptions;
using api.core.Data.requests;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.core.controllers;

[ApiController]
[Route("api/events")]
public class EventsController(ILogger<EventsController> logger, IEventService eventService, IUserService userService) : ControllerBase
{
    /// <summary>
    /// Get events by date, activity area and tags
    /// </summary>
    /// <param name="startDate">if null, will get every event until the first element</param>
    /// <param name="endDate"></param>
    /// <param name="activityAreas"></param>
    /// <param name="tags"></param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<IEnumerable<EventResponseDTO>> GetEvents(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] IEnumerable<string>? activityAreas,
        [FromQuery] IEnumerable<Guid>? tags,
        [FromQuery] PaginationRequest pagination)
    {
        logger.LogInformation("Getting events");
        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(startDate, endDate, activityAreas, tags, State.Published);
        var totalRecords = events.Count();
        var paginatedRes = events
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var response = PaginationHelper.CreatePaginatedReponse(paginatedRes, validFilter, totalRecords, "/api/campaigns");

        return Ok(response);
    }

    [HttpGet("moderator")]
    [Authorize]
    public ActionResult<IEnumerable<EventResponseDTO>> GetEventsModerator(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] IEnumerable<string>? activityAreas,
        [FromQuery] IEnumerable<Guid>? tags,
        [FromQuery] PaginationRequest pagination,
        [FromQuery] State state = State.All
        )
    {
        logger.LogInformation("Getting events");
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);

        if (userService.GetUser(userId).Type != "Moderator")
        {
            throw new UnauthorizedException();
        }

        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(startDate, endDate, activityAreas, tags, state, ignorePublicationDate: true);
        var totalRecords = events.Count();
        var paginatedRes = events
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var response = PaginationHelper.CreatePaginatedReponse(paginatedRes, validFilter, totalRecords, "/api/campaigns");

        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetEvent(Guid id)
    {
        logger.LogInformation($"Getting event {id}");

        var evnt = eventService.GetEvent(id);

        return new OkObjectResult(
            new Response<EventResponseDTO>
            {
                Data = evnt,
            });
    }

    [Authorize]
    [HttpPost]
    public IActionResult AddEvent([FromBody] EventRequestDTO dto)
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

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult DeleteEvent(Guid id)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var isDeleted = eventService.DeleteEvent(userId, id);
        return isDeleted ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPatch("{id}")]
    public IActionResult UpdateEvent(Guid id, [FromBody] EventRequestDTO dto)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return eventService.UpdateEvent(userId, id, dto) ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPatch("{id}/state")]
    public IActionResult UpdateEventState(Guid id, [FromQuery] State newState)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return eventService.UpdateEventState(userId, id, newState) ? Ok() : BadRequest();
    }
}
