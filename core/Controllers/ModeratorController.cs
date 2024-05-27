using api.core.Data;
using api.core.Data.Requests;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

/// <summary>
/// The moderators controller that will handle all the requests related to the moderators.
/// </summary>
/// <param name="moderatorService">allows to, for the moment, only create a new moderator</param>
[ApiController]
[Route("api/moderators")]
public class ModeratorController(IModeratorService moderatorService) : ControllerBase
{
    /// <summary>
    /// This function will create a new moderator in the database. This allow simplifying the starting setup 
    /// setup of the application by allowing having a public endpoint to create a starting moderator.
    /// Without the header x-api-key, this function will not worked.
    /// 
    /// Also, the configuration of the access key is done in the .env file. If you want to remove this functionnality.
    /// Let's say in a production server, simply remove the value ADMIN_ACCESS_API_KEY from the environment variables.
    /// </summary>
    /// <param name="accessKey">The provided access key that needs to match the ADMIN_ACCESS_API_KEY</param>
    /// <param name="moderator">A moderator creation request object</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateModerator([FromHeader(Name = "x-api-key")] string? accessKey, [FromBody] ModeratorCreateRequestDTO moderator)
    {
        // Create the moderator
        try
        {
            if (accessKey == null)
                throw new Exception("Access key not defined, can't trigger this function");

            var moderatorRes = moderatorService.CreateModerator(accessKey, moderator);

            return Ok(new Response<ModeratorResponseDTO>
            {
                Data = moderatorRes
            });
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
