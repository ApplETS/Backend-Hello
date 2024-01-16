using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloAPI.Data.DataModels;

public class Publication
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public string State { get; set; }

    [Required]
    public DateTime PublicationDate { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    public long? ModeratorId { get; set; }
    [ForeignKey("ModeratorId")]
    public Moderator Moderator { get; set; }

    [Required]
    public long OrganizerId { get; set; }
    [ForeignKey("OrganizerId")]
    public Organizer Organizer { get; set; }
}