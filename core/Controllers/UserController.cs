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
public class UserController(IUserService service) : ControllerBase
{
    [HttpPost("organizer")]
    public ActionResult<UserResponseDTO> CreateOrganizer([FromBody] UserRequestDTO organizer)
    {
        var created = service.AddOrganizer(organizer);
        return Ok(created);
    }

    [HttpGet]
    public IActionResult GetUser()
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var organizer = service.GetUser(userId);

        return new OkObjectResult(
             new Response<UserResponseDTO>
             {
                 Data = organizer,
             });
    }

    [HttpPatch]
    public IActionResult UpdateUser([FromBody] UserRequestDTO user)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return service.UpdateUser(userId, user) ? Ok() : BadRequest();
    }
}
