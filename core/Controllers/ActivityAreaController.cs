using api.core.Data;
using api.core.Data.Responses;
using api.core.Services.Abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

/// <summary>
/// This controller is responsible for handling all the requests related to activity areas.
/// 
/// An activity area is a category of activities that are related to each other. For example,
/// an organizer is associated with an activity area, and the events that the organizer creates
/// will be bound to the same activity area.
/// </summary>
/// <param name="activityAreaService">The ActivityAreaService allows to fetch activity Areas</param>
[ApiController]
[Route("api/activity-areas")]
public class ActivityAreaController(IActivityAreaService activityAreaService) : ControllerBase
{
    /// <summary>
    /// Get all the activity areas that are available in the database.
    /// Optionally, you can search for a specific activity area by providing a search query.
    /// </summary>
    /// <param name="search">[Optional] A search term that will allows filtering the activityAreas fetched.</param>
    /// <returns>Returns any activityArea that match the search field if provided, else will return all</returns>
    [HttpGet]
    public IActionResult GetAllActivityAreas([FromQuery] string? search)
    {
        var activityAreas = activityAreaService.GetAllActivityAreas(search);
        return Ok(new Response<IEnumerable<ActivityAreaResponseDTO>>
        {
            Data = activityAreas
        });
    }

    /// <summary>
    /// Get a specific activity area by its id.
    /// </summary>
    /// <param name="id">The activity Area Id</param>
    /// <returns>data of the activityArea</returns>
    [HttpGet("{id}")]
    public IActionResult GetOneActivityArea(Guid id)
    {
        var activityArea = activityAreaService.GetActivityArea(id);
        return Ok(new Response<ActivityAreaResponseDTO>
        {
            Data = activityArea
        });
    }
}
