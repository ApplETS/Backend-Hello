using System.ComponentModel.DataAnnotations.Schema;

using api.core.Data.Entities;


namespace api.core.data.entities;

[Table(nameof(Subscription))]
public partial class Subscription : BaseEntity
{
    public string Email { get; set; } = null!;
    
    public Guid OrganizerId { get; set; }

    public string SubscriptionToken { get; set; } = null!;

    [ForeignKey(nameof(OrganizerId))]
    [InverseProperty(nameof(entities.Organizer.Subscriptions))]
    public virtual Organizer Organizer { get; set; } = null!;

    [InverseProperty(nameof(Notification.Subscription))]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
