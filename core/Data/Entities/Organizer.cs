﻿using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;

namespace api.core.data.entities;

[Table("Organizer")]
public partial class Organizer : User
{
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

    [InverseProperty("Organizer")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();

    [ForeignKey("ActivityAreaId")]
    [InverseProperty("Organizers")]
    public virtual ActivityArea? ActivityArea { get; set; }

    [InverseProperty(nameof(Subscription.Organizer))]
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
