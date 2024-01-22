using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.core.Data.DataModels;


[Table("Report")]
public class Report : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string Reason { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;
    
    public long PublicationId { get; set; }
    
    [ForeignKey("PublicationId")]
    [InverseProperty("Reports")]
    public virtual Publication Publications { get; set; } = null!;

}
