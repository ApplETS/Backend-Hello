using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloAPI.Data.DataModels;

[Table("Organizer")]
public class Organizer : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Organisation { get; set; }

    [Required]
    public string ActivityArea { get; set; }

    [InverseProperty("Organizer")]
    public ICollection<Publication> Publications { get; set; }
}
