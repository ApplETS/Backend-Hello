﻿using api.core.Data;
using api.core.Data.requests;
using api.core.Data.Responses;
using api.core.Misc;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.core.controllers;

[Authorize]
[ApiController]
[Route("api/me")]
public class MeController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUser()
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var organizer = userService.GetUser(userId);

        return new OkObjectResult(
             new Response<UserResponseDTO>
             {
                 Data = organizer,
             });
    }

    [HttpPatch]
    public IActionResult UpdateUser([FromBody] UserUpdateDTO user)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        return userService.UpdateUser(userId, user) ? Ok() : BadRequest();
    }

    /// <summary>
    /// Update the user connected avatar
    /// </summary>
    /// <param name="avatarReq"></param>
    /// <returns>The url of the downloadable avatar file</returns>
    [HttpPatch("avatar")]
    public IActionResult UpdateUserAvatar([FromForm] UserAvatarUpdateDTO avatarReq)
    {
        var userId = JwtUtils.GetUserIdFromAuthHeader(HttpContext.Request.Headers["Authorization"]!);
        var url = userService.UpdateUserAvatar(userId, avatarReq.avatarFile);

        return new OkObjectResult(
            new Response<string>
            {
                Data = url,
            });
    }
}
