using api.core.Data;
using api.core.Data.Responses;
using api.core.Services.Abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

[ApiController]
[Route("api/activity-areas")]
public class ActivityAreaController(IActivityAreaService activityAreaService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllActivityAreas([FromQuery] string? search)
    {
        var activityAreas = activityAreaService.GetAllActivityAreas(search);
        return Ok(new Response<IEnumerable<ActivityAreaResponseDTO>>
        {
            Data = activityAreas
        });
    }

    [HttpGet("{id}")]
    public IActionResult GetAllActivityAreas(Guid id)
    {
        var activityArea = activityAreaService.GetActivityArea(id);
        return Ok(new Response<ActivityAreaResponseDTO>
        {
            Data = activityArea
        });
    }
}
