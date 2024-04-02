using api.core.Data.Requests;
using api.core.Data.Responses;

namespace api.core.services.abstractions;

public interface IReportService
{
    public Task ReportEventAsync(Guid eventId,  CreateReportRequestDTO request);

    public IEnumerable<ReportResponseDTO> GetReports();
}
