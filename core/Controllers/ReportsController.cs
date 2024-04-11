using api.core.controllers;
using api.core.Data;
using api.core.Data.Enums;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;
using api.core.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace api.core.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController(ILogger<ReportsController> logger, IReportService reportService) : ControllerBase
{
    public const string RATE_LIMITING_POLICY_NAME = "ReportsControllerRateLimitPolicy";

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

    [HttpPost("{id}/reports")]
    [EnableRateLimiting(RATE_LIMITING_POLICY_NAME)]
    public IActionResult ReportEvent(Guid id, [FromBody] CreateReportRequestDTO request)
    {
        logger.LogInformation($"Reporting event {id}");

        reportService.ReportEventAsync(id, request);

        return Ok();
    }
}
