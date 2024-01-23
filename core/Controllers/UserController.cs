using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController() : ControllerBase
{
}
