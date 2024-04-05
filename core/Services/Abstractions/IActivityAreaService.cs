using api.core.Data.Responses;

namespace api.core.Services.Abstractions;

public interface IActivityAreaService
{
    public IEnumerable<ActivityAreaResponseDTO> GetAllActivityAreas(string? search);

    public ActivityAreaResponseDTO GetActivityArea(Guid id);
}