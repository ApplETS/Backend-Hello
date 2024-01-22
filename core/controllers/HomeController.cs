using api.core.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

//[Authorize]
[ApiController]
[Route("api/user")]
public class UserController(EventManagementContext context) : ControllerBase
{
    [HttpPost]
    public ActionResult CreateUser()
    {
        var pubs = context.Publications.ToList();
        return Ok();
    }
}
