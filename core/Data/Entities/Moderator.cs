using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace api.core.data.entities;

[Table("Moderator")]
public partial class Moderator : User
{
    [InverseProperty("Moderator")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
