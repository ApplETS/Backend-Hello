using api.core.Data;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace api.core.Controllers;

/// <summary>
/// A controller to handle calls to manage the reports. This controller is only accessible to moderators.
/// 
/// Under the hood, this controller uses the IReportService to manage the data.
/// </summary>
/// <param name="logger">A logger provider to log informations in the controller</param>
/// <param name="reportService">Used to fetch and manage the reports</param>
[ApiController]
[Route("api/reports")]
public class ReportsController(ILogger<ReportsController> logger, IReportService reportService) : ControllerBase
{
    public const string RATE_LIMITING_POLICY_NAME = "ReportsControllerRateLimitPolicy";

    /// <summary>
    /// Get all event reports without filters
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AuthPolicies.IsModerator)]
    [HttpGet]
    public ActionResult<IEnumerable<ReportResponseDTO>> GetEventsReports()
    {
        var reports = reportService.GetReports();

        return Ok(new Response<IEnumerable<ReportResponseDTO>>
        {
            Data = reports,
        });
    }

    /// <summary>
    /// Report an event. This is also part of the public API that anybody can use to report an event.
    /// There is a rate limiting policy applied to this endpoint to prevent abuse.
    /// 
    /// Normally the rate limiting policy would be defined in the RateLimiterExtension file.
    /// </summary>
    /// <param name="id">the id of the event</param>
    /// <param name="request">the request object to create a new report</param>
    /// <returns></returns>
    [HttpPost("{id}")] // TODO: remove id from the path and place it in the body
    [EnableRateLimiting(RATE_LIMITING_POLICY_NAME)]
    public IActionResult ReportEvent(Guid id, [FromBody] CreateReportRequestDTO request)
    {
        logger.LogInformation($"Reporting event {id}");

        reportService.ReportEventAsync(id, request);

        return Ok();
    }
}
