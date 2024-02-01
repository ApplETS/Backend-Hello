namespace api.core.Data.Exceptions;

/// <summary>
/// When you don't have access to a specific resource
/// </summary>
public class UnauthorizedException : HttpException
{
    public UnauthorizedException()
        : base($"Unauthorized access to resources")
    {
        StatusCode = 401;
        ErrorCode = $"UNAUTHORIZED";
    }
}