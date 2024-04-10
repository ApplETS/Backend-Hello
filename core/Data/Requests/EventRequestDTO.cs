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
    /// <summary>
    /// Either this field should be set (not null) or the ImageUrl string field should be set
    /// If none of them are set, the creation will fail
    /// </summary>
    public IFormFile? Image { get; set; }

    /// <summary>
    /// Either this field should be set (not null) or the Image FormFile field should be set
    /// If none of them are set, the creation will fail
    /// </summary>
    public string? ImageUrl { get; set; }
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

    /// <summary>
    /// Either this field should be set (not null) or the ImageUrl string field should be set
    /// If none of them are set, the creation will fail
    /// </summary>
    public IFormFile? Image { get; set; }

    /// <summary>
    /// Either this field should be set (not null) or the Image FormFile field should be set
    /// If none of them are set, the creation will fail
    /// </summary>
    public string? ImageUrl { get; set; }

    public virtual ICollection<Guid> Tags { get; set; } = new List<Guid>();
}