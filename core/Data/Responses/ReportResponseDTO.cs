using api.core.data.entities;

namespace api.core.Data.Responses;

public class ReportResponseDTO
{
    public Guid Id { get; set; }

    public string Reason { get; set; }

    public DateTime Date { get; set; }

    public EventResponseDTO Publication { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public static ReportResponseDTO Map(Report report)
    {
        return new ReportResponseDTO
        {
            Id = report.Id,
            Reason = report.Reason,
            Date = report.Date,
            CreatedAt = report.CreatedAt,
            UpdatedAt = report.UpdatedAt,
            Publication = new EventResponseDTO
            {
                Id = report.Publication.Id,
                Title = report.Publication.Title,
                Content = report.Publication.Content,
                ImageUrl = report.Publication.ImageUrl,
                ImageAltText = report.Publication.ImageAltText,
                Tags = report.Publication.Tags.Select(TagResponseDTO.Map),
                State = report.Publication.State,
                PublicationDate = report.Publication.PublicationDate,
                CreatedAt = report.Publication.CreatedAt,
                UpdatedAt = report.Publication.UpdatedAt,
            }
        };
    }
}
