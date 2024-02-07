using api.core.data.entities;

namespace api.core.Data.Responses;

public class UserResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Organisation { get; set; }

    public string? ActivityArea { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public static UserResponseDTO Map(Organizer organizer)
    {
        return new UserResponseDTO
        {
            Id = organizer.Id,
            Email = organizer.Email,
            Type = "Organizer",
            Organisation = organizer.Organisation,
            ActivityArea = organizer.ActivityArea,
            CreatedAt = organizer.CreatedAt,
            UpdatedAt = organizer.UpdatedAt
        };
    }

    public static UserResponseDTO Map(Moderator moderator)
    {
        return new UserResponseDTO
        {
            Id = moderator.Id,
            Email = moderator.Email,
            Type = "Moderator",
            CreatedAt = moderator.CreatedAt,
            UpdatedAt = moderator.UpdatedAt
        };
    }
}
