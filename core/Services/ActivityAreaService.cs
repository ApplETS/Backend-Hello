using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.services.abstractions;
using api.core.Services.Abstractions;

using Supabase;

namespace api.core.Services;

public class ActivityAreaService(IActivityAreaRepository activityAreaRepository) : IActivityAreaService
{
    public IEnumerable<ActivityAreaResponseDTO> GetAllActivityAreas(string? search)
    {
        return activityAreaRepository.GetAll()
            .Where(aa =>
            (aa.NameFr.Contains(search ?? "", StringComparison.InvariantCultureIgnoreCase) ||
            aa.NameEn.Contains(search ?? "", StringComparison.InvariantCultureIgnoreCase)) &&
            aa.DeletedAt == null)
            .Select(ActivityAreaResponseDTO.Map);
    }

    public ActivityAreaResponseDTO GetActivityArea(Guid id)
    {
        var activityArea = activityAreaRepository.Get(id);
        NotFoundException<ActivityArea>.ThrowIfNull(activityArea);

        if (activityArea!.DeletedAt != null)
            throw new NotFoundException<ActivityArea>();

        return ActivityAreaResponseDTO.Map(activityArea!);
    }

}