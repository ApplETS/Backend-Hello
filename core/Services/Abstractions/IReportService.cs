using api.core.Data.Requests;
using api.core.Data.Responses;

namespace api.core.Services.Abstractions;

public interface IReportService
{
    public void ReportEvent(Guid eventId,  CreateReportRequestDTO request);

    public IEnumerable<ReportResponseDTO> GetReports();
}
