using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace HelloAPI.Data.Entities;

[Table("Event")]
public partial class Event
{
    [Key]
    public long Id { get; set; }

    public DateTime EventDate { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("Event")]
    public virtual Publication IdNavigation { get; set; } = null!;
}
