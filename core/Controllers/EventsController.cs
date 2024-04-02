using api.core.Data;
using api.core.Data.Enums;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;

namespace api.core.controllers;

[ApiController]
[Route("api/events")]
public class EventsController(ILogger<EventsController> logger, IEventService eventService, ITagService tagService, IReportService reportService) : ControllerBase
{

    public const string RATE_LIMITING_POLICY_NAME = "EventsControllerRateLimitPolicy";

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
        [FromQuery] IEnumerable<string>? activityAreas,
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
            .ToList();

        var response = PaginationHelper.CreatePaginatedReponse(paginatedRes, validFilter, totalRecords);

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

    [HttpPost("{id}/reports")]
    [EnableRateLimiting(RATE_LIMITING_POLICY_NAME)]
    public IActionResult ReportEvent(Guid id, [FromBody] CreateReportRequestDTO request)
    {
        logger.LogInformation($"Reporting event {id}");

        reportService.ReportEventAsync(id, request);

        return Ok();
    }

    [HttpGet("tags")]
    public IActionResult GetTags()
    {
        var tags = tagService.GetTags();
        return Ok(new Response<IEnumerable<TagResponseDTO>>
        {
            Data = tags,
        });
    }
}
