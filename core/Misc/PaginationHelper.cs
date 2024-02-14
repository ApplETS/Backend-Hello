using api.core.Data;
using api.core.Data.Requests;

namespace api.core.Misc;

public class PaginationHelper
{

    public static PaginatedResponse<List<T>> CreatePaginatedReponse<T>(List<T> pagedData, PaginationRequest validFilter, int totalRecords)
    {
        var response = new PaginatedResponse<List<T>>(pagedData, validFilter.PageNumber, pagedData.Count());
        var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;
        return response;
    }
}

