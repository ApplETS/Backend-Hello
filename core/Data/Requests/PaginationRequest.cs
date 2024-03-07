using Microsoft.VisualBasic;

namespace api.core.Data.Requests;

/// <summary>
/// This class can be passed as parameter in endpoint to 
/// </summary>
public class PaginationRequest
{
    public const int MINIMUM_DEFAULT_PAGE_SIZE = 3;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = MINIMUM_DEFAULT_PAGE_SIZE;

    public PaginationRequest()
    { }

    public PaginationRequest(int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
        this.PageSize = pageSize <= MINIMUM_DEFAULT_PAGE_SIZE ? MINIMUM_DEFAULT_PAGE_SIZE : pageSize;
    }
}
