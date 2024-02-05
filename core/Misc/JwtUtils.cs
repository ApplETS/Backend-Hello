using System.IdentityModel.Tokens.Jwt;

namespace api.core.Misc;

public static class JwtUtils
{
    public static Guid GetUserIdFromAuthHeader(string authHeader)
    {
        var token = authHeader.Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
        {
            throw new ArgumentException("Invalid JWT token");
        }

        var payload = jsonToken.Payload;
        if (payload.TryGetValue("sub", out var userIdValue))
        {
            if (Guid.TryParse(userIdValue.ToString(), out Guid userId))
            {
                return userId;
            }
            else
            {
                throw new ArgumentException("sub is not in a valid Guid format");
            }
        }
        else
        {
            throw new ArgumentException("sub not found in JWT payload");
        }
    }
}
