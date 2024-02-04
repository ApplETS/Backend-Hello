using api.core.data.entities;

namespace api.core.Data.requests;

public class EventRequestDTO
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string State { get; set; } = null!;
    
    public DateTime PublicationDate { get; set; }

    public DateTime EventDate { get; set; }

    public virtual ICollection<Guid> Tags { get; set; } = new List<Guid>();
}
