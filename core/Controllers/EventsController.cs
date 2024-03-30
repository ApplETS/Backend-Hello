using api.core.data.entities;
using api.core.Data;
using api.core.Data.Enums;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;
using api.emails.Models;
using api.emails.Services;
using api.emails.Services.Abstractions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;

namespace api.core.controllers;

[ApiController]
[Route("api/events")]
public class EventsController(ILogger<EventsController> logger, IEventService eventService, ITagService tagService, IReportService reportService, IEmailService emailService) : ControllerBase
{

    public const string RATE_LIMITING_POLICY_NAME = "EventsControllerRateLimitPolicy";

    /// <summary>
    /// Get events by date, activity area and tags
    /// </summary>
    /// <param name="startDate">if null, will get every event until the first element</param>
    /// <param name="endDate">if null, will get every element unitl infinity</param>
    /// <param name="organizerId">Filter by organizerId</param>
    /// <param name="activityAreas">Filter by a list of OR applicable activityAreas</param>
    /// <param name="tags">Filter by a list of OR applicable tags</param>
    /// <param name="pagination">Sort and take only the necessary page</param>
    /// <returns>events filtered and sorted</returns>
    [HttpGet]
    [OutputCache(VaryByQueryKeys = [ "startDate", "endDate", "activityAreas", "tags", "pageNumber", "pageSize" ])]
    public ActionResult<IEnumerable<EventResponseDTO>> GetEvents(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] Guid? organizerId,
        [FromQuery] IEnumerable<string>? activityAreas,
        [FromQuery] IEnumerable<Guid>? tags,
        [FromQuery] PaginationRequest pagination)
    {
        logger.LogInformation("Getting events");
        var validFilter = new PaginationRequest(pagination.PageNumber, pagination.PageSize);

        var events = eventService.GetEvents(startDate, endDate, activityAreas, tags, organizerId, State.Published);
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
    public async Task<IActionResult> ReportEvent(Guid id, [FromBody] CreateReportRequestDTO request)
    {
        logger.LogInformation($"Reporting event {id}");

        reportService.ReportEvent(id, request);

        var evnt = eventService.GetEvent(id);

        if(evnt.ReportCount > 5) // TODO: Add constant
        {
            await emailService.SendEmailAsync(
            "moderator@email", // TODO: Moderator email
            $"Alerte de signalements: {evnt.Title}",
            new ReportModel
            {
                Title = "Alerte de signalement",
                Salutation = $"Bonjour Moderateur,", // TODO: Moderateur name
                AlertSubject = "Alerte de rapports d'événement",
                AlertMessage = "L'événement suivant a reçu plusieurs rapports:",
                EventTitleHeader = "Titre de l'événement: ",
                EventTitle = "Titre de l'événement ici", // Replace with actual event title
                NumberOfReportsHeader = "Nombre de rapports: ",
                NumberOfReports = 5, // Replace with actual number of reports
                ActionRequiredMessage = "Veuillez prendre les mesures nécessaires.",
                ViewEventButtonText = "Voir l'événement",
                EventLink = new Uri("URL_de_votre_evenement") // Replace with actual event URL
            },
            emails.EmailsUtils.ReportTemplate
        );
        }

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
