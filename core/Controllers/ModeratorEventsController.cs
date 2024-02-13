﻿using api.core.controllers;
using api.core.Data.Entities;
using api.core.Data.Exceptions;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;
using api.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

[ApiController]
[Route("api/moderator/events")]
public class ModeratorEventsController(ILogger<ModeratorEventsController> logger, IEventService eventService, IUserService userService) : ControllerBase
{
    [Authorize]
    [HttpPatch("{id}/state")]
    public IActionResult UpdateEventState(Guid id, [FromQuery] State newState)
    {
        EnsureIsModerator();
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return eventService.UpdateEventState(userId, id, newState) ? Ok() : BadRequest();
    }

    [Authorize]
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
        EnsureIsModerator();

        logger.LogInformation("Getting events");

        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(startDate, endDate, activityAreas, tags, state, ignorePublicationDate: true);
        var totalRecords = events.Count();
        var paginatedRes = events
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        var response = PaginationHelper.CreatePaginatedReponse(paginatedRes, validFilter, totalRecords);

        return Ok(response);
    }

    private void EnsureIsModerator()
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);

        if (userService.GetUser(userId).Type != "Moderator")
        {
            throw new UnauthorizedException();
        }
    }
}
