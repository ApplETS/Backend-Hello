using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HelloAPI.Data.Entities;

[Table("userprofile")]
public partial class UserProfile
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("activity_sector")]
    public required string ActivitySector { get; set; }

    [Required]
    [Column("organisation")]
    public required string Organisation { get; set; }

    [Column("profile_picture_url")]
    public string? ProfilePictureURL { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
