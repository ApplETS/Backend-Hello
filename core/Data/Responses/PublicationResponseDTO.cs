using api.core.data.entities;

namespace api.core.Data.Responses;

public class PublicationResponseDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string State { get; set; } = null!;

    public DateTime PublicationDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ModeratorResponseDTO? Moderator { get; set; }

    public OrganizerResponseDTO Organizer { get; set; } = null!;

    public virtual ICollection<Guid> Tags { get; set; } = new List<Guid>();

    public static PublicationResponseDTO Map(Event oneEvent)
    {
        return new PublicationResponseDTO
        {
            Id = oneEvent.Id,
            Title = oneEvent.Publication.Title,
            Content = oneEvent.Publication.Content,
            ImageUrl = oneEvent.Publication.ImageUrl,
            State = oneEvent.Publication.State,
            PublicationDate = oneEvent.Publication.PublicationDate,
            CreatedAt = oneEvent.Publication.CreatedAt,
            UpdatedAt = oneEvent.Publication.UpdatedAt,
            Moderator = oneEvent.Publication.Moderator != null ? ModeratorResponseDTO.Map(oneEvent.Publication.Moderator!) : null,
            Organizer = OrganizerResponseDTO.Map(oneEvent.Publication.Organizer),
        };
    }
}
