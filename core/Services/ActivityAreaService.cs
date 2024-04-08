using api.core.data.entities;
using api.core.Data.Exceptions;
using api.core.Data.Responses;
using api.core.repositories.abstractions;
using api.core.Services.Abstractions;

using Microsoft.IdentityModel.Tokens;

namespace api.core.Services;

public class ActivityAreaService(IActivityAreaRepository activityAreaRepository) : IActivityAreaService
{
    public IEnumerable<ActivityAreaResponseDTO> GetAllActivityAreas(string? search)
    {
        return activityAreaRepository.GetAll()
            .Where(aa =>
            (search.IsNullOrEmpty() || 
                aa.NameFr.ToLower().Contains(search!.ToLower() ?? "") ||
                aa.NameEn.ToLower().Contains(search!.ToLower() ?? "")) &&
            aa.DeletedAt == null)
            .ToList()
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