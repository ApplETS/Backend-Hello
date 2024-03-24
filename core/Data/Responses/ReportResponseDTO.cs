using api.core.data.entities;
using api.core.Data.Enums;

namespace api.core.Data.Responses;

public class ReportResponseDTO
{
    public required Guid Id { get; set; }

    public required string Reason { get; set; }

    public required ReportCategory Category { get; set; }

    public required EventResponseDTO Publication { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required DateTime UpdatedAt { get; set; }

    public static ReportResponseDTO Map(Report report)
    {
        return new ReportResponseDTO
        {
            Id = report.Id,
            Reason = report.Reason,
            Category = report.Category,
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
