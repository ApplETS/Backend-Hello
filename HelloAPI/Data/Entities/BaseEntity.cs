using System.ComponentModel.DataAnnotations;

namespace HelloAPI.Data.Entities;

public abstract class BaseEntity
{
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }
}