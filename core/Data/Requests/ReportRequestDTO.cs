using api.core.Data.Enums;

namespace api.core.Data.Requests;

public class CreateReportRequestDTO
{
    public string Reason { get; set; }

    public ReportCategory Category { get; set; }
}
