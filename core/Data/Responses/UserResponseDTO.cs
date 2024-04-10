using api.core.data.entities;

namespace api.core.Data.Responses;

public class UserResponseDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string AvatarUrl { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Organization { get; set; }

    public ActivityAreaResponseDTO? ActivityArea { get; set; }

    public IEnumerable<FieldOfInterestTagResponseDTO>? FieldsOfInterests { get; set; }

    public bool IsActive { get; set; }

    public bool HasLoggedIn { get; set; }

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
            Organization = organizer.Organization,
            ActivityArea = organizer.ActivityArea != null ?
                ActivityAreaResponseDTO.Map(organizer.ActivityArea) :
                null,
            IsActive = organizer.IsActive,
            HasLoggedIn = organizer.HasLoggedIn,
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
            IsActive = true,
            HasLoggedIn = true,
            CreatedAt = moderator.CreatedAt,
            UpdatedAt = moderator.UpdatedAt
        };
    }
}
