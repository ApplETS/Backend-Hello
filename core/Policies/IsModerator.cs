using System.Security.Claims;

using api.core.Data.Exceptions;
using api.core.services.abstractions;

using Microsoft.AspNetCore.Authorization;

namespace api.core.Policies;

public class IsModeratorRequirement : IAuthorizationRequirement { }

public class IsModeratorHandler(IUserService userService) : AuthorizationHandler<IsModeratorRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsModeratorRequirement requirement)
    {
        // TODO : Remplacer cette politique autrement
        var identifierClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //var userId = Guid.Parse(identifierClaim ?? throw new UnauthorizedException());

        var user = userService.GetUser(identifierClaim);
        if (user != null && user.Type != "Moderator")
            throw new UnauthorizedException();
        else
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
