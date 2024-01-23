using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

//[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(IOrganizerService service) : ControllerBase
{
    [HttpPost("organizer")]
    public ActionResult<OrganizerResponseDTO> CreateOrganizer([FromBody] OrganizerCreationRequestDTO organizer)
    {
        var created = service.AddOrganizer(organizer);
        return Ok(created);
    }
}
