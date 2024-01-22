using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace api.core.Data.Entities;

[Table("Report")]
[Index("PublicationId", Name = "IX_Report_PublicationId")]
public partial class Report
{
    [Key]
    public long Id { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime Date { get; set; }

    public long PublicationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    [ForeignKey("PublicationId")]
    [InverseProperty("Reports")]
    public virtual Publication Publication { get; set; } = null!;
}
