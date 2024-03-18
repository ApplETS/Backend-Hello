using api.core.data.entities;
using api.core.Data.Entities;

namespace api.core.Data.Responses;

public class EventResponseDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public State State { get; set; }

    public string? Reason{ get; set; }

    public DateTime PublicationDate { get; set; }

    public DateTime EventStartDate { get; set; }

    public DateTime EventEndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public UserResponseDTO? Moderator { get; set; }

    public UserResponseDTO Organizer { get; set; } = null!;

    public virtual IEnumerable<TagResponseDTO> Tags { get; set; } = new List<TagResponseDTO>();

    public static EventResponseDTO Map(Event oneEvent)
    {
        return new EventResponseDTO
        {
            Id = oneEvent.Id,
            Title = oneEvent.Publication.Title,
            Content = oneEvent.Publication.Content,
            ImageUrl = oneEvent.Publication.ImageUrl,
            Tags = oneEvent.Publication.Tags.Select(TagResponseDTO.Map),
            State = oneEvent.Publication.State,
            Reason = oneEvent.Publication.Reason,
            PublicationDate = oneEvent.Publication.PublicationDate,
            EventStartDate = oneEvent.EventStartDate,
            EventEndDate = oneEvent.EventEndDate,
            CreatedAt = oneEvent.Publication.CreatedAt,
            UpdatedAt = oneEvent.Publication.UpdatedAt,
            Moderator = oneEvent.Publication.Moderator != null ? UserResponseDTO.Map(oneEvent.Publication.Moderator!) : null,
            Organizer = UserResponseDTO.Map(oneEvent.Publication.Organizer),
        };
    }
}
