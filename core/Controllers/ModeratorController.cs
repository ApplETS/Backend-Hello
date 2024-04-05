using api.core.Data;
using api.core.Data.Requests;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

[ApiController]
[Route("api/moderator")]
public class ModeratorController(IModeratorService moderatorService) : ControllerBase
{
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
