using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;

namespace api.core.data.entities;

[Table("Organizer")]
public partial class Organizer : User
{
    public string Organization { get; set; } = null!;

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

    [InverseProperty("Organizer")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();

    [ForeignKey("ActivityAreaId")]
    [InverseProperty("Organizer")]
    public virtual ActivityArea ActivityArea { get; set; }
}
