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

    public string ProfileDescription { get; set; } = null!;

    public string? FacebookLink { get; set; }

    public string? InstagramLink { get; set; }

    public string? TikTokLink { get; set; }

    public string? XLink { get; set; }

    public string? DiscordLink { get; set; }

    public string? LinkedInLink { get; set; }

    public string? RedditLink { get; set; }

    public string? WebSiteLink { get; set; }

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
            ProfileDescription = organizer.ProfileDescription,
            FacebookLink = organizer.FacebookLink,
            InstagramLink = organizer.InstagramLink,
            TikTokLink = organizer.TikTokLink,
            XLink = organizer.XLink,
            DiscordLink = organizer.DiscordLink,
            LinkedInLink = organizer.LinkedInLink,
            RedditLink = organizer.RedditLink,
            WebSiteLink = organizer.WebSiteLink,
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
