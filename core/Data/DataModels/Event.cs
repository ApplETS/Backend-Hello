using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.core.Data.DataModels;

[Table("Event")]
public class Event : Publication
{
    [Required]
    public DateTime EventDate { get; set; }
}
