namespace api.core.Data.Requests;

public class OrderingRequest
{
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = false;
}
