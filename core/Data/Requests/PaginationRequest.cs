using Microsoft.VisualBasic;

namespace api.core.Data.Requests;

/// <summary>
/// This class can be passed as parameter in endpoint to 
/// </summary>
public class PaginationRequest
{
    public const int MINIMUM_DEFAULT_PAGE_SIZE = 10;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PaginationRequest()
    { }

    public PaginationRequest(int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
        this.PageSize = pageSize <= MINIMUM_DEFAULT_PAGE_SIZE ? MINIMUM_DEFAULT_PAGE_SIZE : pageSize;
    }
}
