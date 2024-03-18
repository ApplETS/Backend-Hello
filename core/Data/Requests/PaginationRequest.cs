using Microsoft.VisualBasic;

namespace api.core.Data.Requests;

/// <summary>
/// This class can be passed as parameter in endpoint to 
/// </summary>
public class PaginationRequest
{
    public const int DEFAULT_PAGE_SIZE = 3;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;

    public PaginationRequest()
    { }

    public PaginationRequest(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize <= 0 ? DEFAULT_PAGE_SIZE : pageSize;
    }
}
