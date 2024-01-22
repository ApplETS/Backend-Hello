using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloAPI.Data.Entities;

[Table("Event")]
public class Event : Publication
{
    [Required]
    public DateTime EventDate { get; set; }
}
