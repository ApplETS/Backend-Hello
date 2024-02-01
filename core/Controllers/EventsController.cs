using api.core.Data.Responses;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

[ApiController]
[Route("api/events")]
public class EventsController(ILogger<EventsController> logger, IEventService eventService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<EventResponseDTO>> GetEvents(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] IEnumerable<string>? tags)
    {
        var events = eventService.GetEvents(startDate, endDate, tags);
        return Ok(events);
    }
}
