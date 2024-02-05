using api.core.data.entities;
using api.core.Data;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(IOrganizerService service) : ControllerBase
{
    [HttpPost("organizer")]
    public ActionResult<OrganizerResponseDTO> CreateOrganizer([FromBody] OrganizerRequestDTO organizer)
    {
        var created = service.AddOrganizer(organizer);
        return Ok(created);
    }

    [HttpGet("organizer")]
    public IActionResult GetOrganizer()
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var organizer = service.GetOrganizer(userId);

        return new OkObjectResult(
             new Response<OrganizerResponseDTO>
             {
                 Data = organizer,
             });
    }

    [HttpPatch("organizer")]
    public IActionResult GetOrganizer([FromBody] OrganizerRequestDTO organizer)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return service.UpdateOrganizer(userId, organizer) ? Ok() : BadRequest();
    }
}
