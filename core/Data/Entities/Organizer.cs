using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.core.data.entities;

[Table("Organizer")]
public partial class Organizer
{
    [Key]
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Organisation { get; set; } = null!;

    public string ActivityArea { get; set; } = null!;

    public string ProfileDescription { get; set; } = null!;

    public bool IsActive { get; set; }

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

    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Organizer")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
