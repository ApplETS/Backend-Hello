using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using api.core.data.entities;

namespace api.core.Data.Entities;

[Table(nameof(User))]
public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid? ActivityAreaId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public UserRole Role { get; set; }

    // Propriétés de Organizer
    public string Organization { get; set; } = null!;

    public string ProfileDescription { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool HasLoggedIn { get; set; }

    public string? FacebookLink { get; set; }

    public string? InstagramLink { get; set; }

    public string? TikTokLink { get; set; }

    public string? XLink { get; set; }

    public string? DiscordLink { get; set; }

    public string? LinkedInLink { get; set; }

    public string? RedditLink { get; set; }

    public string? WebSiteLink { get; set; }

    [InverseProperty(nameof(Subscription.Organizer))]
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    // Adaptation personalisée
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}

[Flags]
public enum UserRole
{
    Admin = 0b00000001,
    Moderator = 0b00000010,
    Organizer = 0b00000100
}