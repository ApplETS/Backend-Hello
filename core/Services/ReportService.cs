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
}
