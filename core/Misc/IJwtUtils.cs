namespace api.core.Misc;

public interface IJwtUtils
{
    string GetUserIdFromAuthHeader(string authHeader);
}