using api.core.Data.Enums;

namespace api.core.Data.Requests;

public class CreateReportRequestDTO
{
    public required string Reason { get; set; }

    public required ReportCategory Category { get; set; }
}
