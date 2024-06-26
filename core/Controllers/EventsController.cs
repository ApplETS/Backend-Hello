using api.core.Data;
using api.core.Data.Enums;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace api.core.controllers;

/// <summary>
/// One of the most important controller in the system, it handles the events/publications.
/// This route is public and can be accessed by anyone. This is mostly what the mobile app will 
/// consume and the open data API accessible from the front-end or external parties that want to
/// show data.
/// 
/// Under the hood, this controller uses the IEventService to manage the event data fetching, as well as
/// the IUserService to fetch the organizer's avatar url for each event returned.
/// </summary>
/// <param name="logger">A logger provider to log informations in the controller</param>
/// <param name="eventService">Used to fetch events</param>
/// <param name="userService">Used to fetch the avatar url of the organiser for each event fetched</param>
[ApiController]
[Route("api/events")]
public class EventsController(
    ILogger<EventsController> logger,
    IEventService eventService,
    IUserService userService) : ControllerBase
{

    /// <summary>
    /// Get events by date, activity area and tags
    /// </summary>
    /// <param name="startDate">if null, will get every event until the first element</param>
    /// <param name="endDate">if null, will get every element unitl infinity</param>
    /// <param name="organizerId">Filter by organizerId</param>
    /// <param name="title">Filter by title</param>
    /// <param name="activityAreas">Filter by a list of OR applicable activityAreas</param>
    /// <param name="tags">Filter by a list of OR applicable tags</param>
    /// <param name="pagination">Sort and take only the necessary page</param>
    /// <returns>events filtered and sorted</returns>
    [HttpGet]
    [OutputCache(VaryByQueryKeys = [ "startDate", "endDate", "organizerId", "title", "activityAreas", "tags", "pageNumber", "pageSize" ])]
    public ActionResult<IEnumerable<EventResponseDTO>> GetEvents(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] Guid? organizerId,
        [FromQuery] string? title,
        [FromQuery] IEnumerable<Guid>? activityAreas,
        [FromQuery] IEnumerable<Guid>? tags,
        [FromQuery] PaginationRequest pagination)
    {
        logger.LogInformation("Getting events");
        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(startDate, endDate, activityAreas, tags, organizerId, title, State.Published);
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

    /// <summary>
    /// Get one event by it's id to show every detail about it.
    /// </summary>
    /// <param name="id">An event id provided</param>
    /// <returns>The event data</returns>
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

}
