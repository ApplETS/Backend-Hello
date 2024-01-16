using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloAPI.Data.DataModels;

[Table("Moderator")]
public class Moderator : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [InverseProperty("Moderator")]
    public virtual ICollection<Publication> Publications { get; set; }
}
