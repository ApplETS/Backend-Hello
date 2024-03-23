using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;


namespace api.core.Services;

public class ReportService(IEventRepository eventRepository, IReportRepository reportRepository) : IReportService
{
    public IEnumerable<ReportResponseDTO> GetReports()
    {
        var reports = reportRepository.GetAll();
        return reports.Select(ReportResponseDTO.Map).ToList();
    }

    public void ReportEvent(Guid eventId, CreateReportRequestDTO request)
    {
        if (AvoidDuplicates(request)) return;

        var evnt = eventRepository.Get(eventId);
        NotFoundException<Event>.ThrowIfNull(evnt);

        var report = new Report
        {
            PublicationId = eventId,
            Reason = request.Reason,
            Date = request.Date,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        reportRepository.Add(report);
    }

    private bool AvoidDuplicates(CreateReportRequestDTO request)
    {
        // If a request with a duplicated reason is submitted within the time window of the rate limiter,
        // we ignore the request
        var timeWindow = int.Parse(Environment.GetEnvironmentVariable("RATE_LIMIT_TIME_WINDOW_SECONDS") ?? "10");
        var reports = reportRepository.GetRecentReports(timeWindow);
        return reports.Any(r => r.Reason == request.Reason);
    }
}
