namespace api.core.Data.Responses;

/// <summary>
/// A common data response to return to the front end. Data or Error should be set but not
/// at the same time.
/// </summary>
/// <typeparam name="T">The object you want to return</typeparam>
public class PaginatedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public PaginatedResponse(T data, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;

        Data = data;
    }
}
