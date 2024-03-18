using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace api.core.data.entities;

[Table("Publication")]
[Index("ModeratorId", Name = "IX_Publication_ModeratorId")]
[Index("OrganizerId", Name = "IX_Publication_OrganizerId")]
public partial class Publication
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? Reason { get; set; }
  
    public string ImageAltText { get; set; } = null!;

    public State State { get; set; }

    public DateTime PublicationDate { get; set; }

    public Guid? ModeratorId { get; set; }

    public Guid OrganizerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Publication")]
    public virtual Event? Event { get; set; }

    [ForeignKey("ModeratorId")]
    [InverseProperty("Publications")]
    public virtual Moderator? Moderator { get; set; }

    [ForeignKey("OrganizerId")]
    [InverseProperty("Publications")]
    public virtual Organizer Organizer { get; set; } = null!;

    [InverseProperty("Publication")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    [ForeignKey("PublicationsId")]
    [InverseProperty("Publications")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
