using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.Requests;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.emails.Models;
using api.emails.Services.Abstractions;


namespace api.core.Services;

public class ReportService(IEventRepository eventRepository, IEventService eventService, IEmailService emailService, IReportRepository reportRepository) : IReportService
{
    public IEnumerable<ReportResponseDTO> GetReports()
    {
        var reports = reportRepository.GetAll();
        return reports.Select(ReportResponseDTO.Map).ToList();
    }

    public async Task ReportEventAsync(Guid eventId, CreateReportRequestDTO request)
    {
        if (AvoidDuplicates(eventId, request)) return;

        var evnt = eventRepository.Get(eventId);
        NotFoundException<Event>.ThrowIfNull(evnt);

        var report = new Report
        {
            PublicationId = eventId,
            Reason = request.Reason,
            Category = request.Category,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        reportRepository.Add(report);

        eventService.UpdateEventReportCount(eventId);
        await CheckAndSendReportEmailAsync(evnt);

    }

    private async Task CheckAndSendReportEmailAsync(Event? evnt)
    {
        var frontBaseUrl = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL") ?? throw new Exception("FRONTEND_BASE_URL is not set");
        var reportCountUntilEmail = Environment.GetEnvironmentVariable("REPORT_COUNT_UNTIL_EMAIL") ?? "5";
        if (evnt?.Publication.ReportCount >= int.Parse(reportCountUntilEmail) && !evnt.Publication.HasBeenReported)
        {
            var res = await emailService.SendEmailAsync(
                evnt.Publication.Moderator!.Email,
                $"Alerte de signalements: {evnt.Publication.Title}",
                new ReportModel
                {
                    Title = "Alerte de signalement",
                    Salutation = $"Bonjour {evnt.Publication.Moderator.Email},",
                    AlertSubject = "Alerte de rapports d'événement",
                    AlertMessage = "L'événement suivant a reçu plusieurs rapports:",
                    EventTitleHeader = "Titre de l'événement: ",
                    EventTitle = $"{evnt.Publication.Title}",
                    NumberOfReportsHeader = "Nombre de rapports: ",
                    NumberOfReports = evnt.Publication.ReportCount,
                    ActionRequiredMessage = "Veuillez prendre les mesures nécessaires.",
                    ViewEventButtonText = "Voir l'événement",
                    EventLink = new Uri($"{frontBaseUrl}/fr/dashboard/approbations?id={evnt.Id}")
                },
                emails.EmailsUtils.ReportTemplate
            );
            if ((res != null) && res.Successful)
            {
                eventService.UpdatePublicationHasBeenReported(evnt.Id);
            }
        }
    }

    private bool AvoidDuplicates(Guid publicationId, CreateReportRequestDTO request)
    {
        // If a request with a duplicated reason, publication id anbd category is submitted within the time window of the rate limiter,
        // we ignore the request
        var timeWindow = int.Parse(Environment.GetEnvironmentVariable("RATE_LIMIT_TIME_WINDOW_SECONDS") ?? "10");
        var reports = reportRepository.GetRecentReports(timeWindow);
        return reports.Any(r => r.Reason == request.Reason && r.PublicationId == publicationId && r.Category == request.Category);
    }
}
