using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloAPI.Data.Entities;

[Table("Publication")]
public abstract class Publication : BaseEntity
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

    public long? ModeratorId { get; set; }
    
    [ForeignKey("ModeratorId")]
    [InverseProperty("Publications")]
    public Moderator Moderator { get; set; }

    [Required]
    public long OrganizerId { get; set; }
    
    [ForeignKey("OrganizerId")]
    [InverseProperty("Publications")]
    public Organizer Organizer { get; set; }
    
    [InverseProperty("Publications")]
    public virtual ICollection<Tag> Tags { get; set; }
        
    [InverseProperty("Publications")]
    public virtual ICollection<Report> Reports { get; } = new List<Report>();
}