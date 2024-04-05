namespace api.core.Data.requests;

public class EventRequestDTO
{
    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int ReportCount { get; set; }

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

public class DraftEventCreationRequestDTO
{
    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? PublicationDate { get; set; }

    public DateTime? EventStartDate { get; set; }

    public DateTime? EventEndDate { get; set; }

    public string? ImageAltText { get; set; }

    public IFormFile? Image { get; set; }

    public virtual ICollection<Guid> Tags { get; set; } = new List<Guid>();
}