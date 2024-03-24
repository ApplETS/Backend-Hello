namespace api.core.Data.requests;

public class EventRequestDTO
{
    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime PublicationDate { get; set; }

    public DateTime EventStartDate { get; set; }

    public DateTime EventEndDate { get; set; }

    public virtual ICollection<Guid> Tags { get; set; } = new List<Guid>();

    public string ImageAltText { get; set; } = null!;
}


public class EventCreationRequestDTO : EventRequestDTO
{
    public IFormFile Image { get; set; } = null!;
}

public class EventUpdateRequestDTO : EventRequestDTO
{
    public IFormFile? Image { get; set; }
}

