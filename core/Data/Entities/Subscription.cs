using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;


namespace api.core.data.entities;

[Table("Subscription")]
public partial class Subscription : BaseEntity
{
    public string Email { get; set; } = null!;
    
    public Guid OrganizerId { get; set; }

    public string SubscriptionToken { get; set; } = null!;

    [InverseProperty("Subscriptions")]
    public virtual Organizer Organizers { get; set; } = null!;
}
