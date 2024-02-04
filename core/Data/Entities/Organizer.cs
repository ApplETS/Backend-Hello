﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.core.data.entities;

[Table("Organizer")]
public partial class Organizer
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Organisation { get; set; } = null!;

    public string ActivityArea { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Organizer")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
