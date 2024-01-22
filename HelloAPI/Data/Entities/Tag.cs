using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloAPI.Data.Entities;

[Table("Tag")]
public class Tag : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int PriorityValue { get; set; }
    
    [InverseProperty("Tags")]
    public virtual ICollection<Publication> Publications { get; set; }
    
    public virtual ICollection<Tag> ParentTags { get; set; }
    
    public virtual ICollection<Tag> ChildrenTags { get; set; }
}
