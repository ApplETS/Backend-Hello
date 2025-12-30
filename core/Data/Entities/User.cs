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

    [ForeignKey(nameof(ActivityAreaId))]
    [InverseProperty(nameof(ActivityArea.Users))]
    public virtual ActivityArea? ActivityArea { get; set; }

    [InverseProperty(nameof(Subscription.Organizer))]
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    // Il ne semble pas nécessaire de mapper les publications dans le User pour le moment
    //public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}

/// <summary>
/// Specifies the roles that a user can have within the system.
/// </summary>
/// <remarks>This enumeration supports bitwise combination of its member values. A User therefore can have multiple roles.</remarks>
[Flags]
public enum UserRole
{
    Admin     = 0b10000000,
    Moderator = 0b01000000,
    Organizer = 0b00100000
}