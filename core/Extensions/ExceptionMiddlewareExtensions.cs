using api.core.Misc;

namespace api.core.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder iBuilder)
    {
        return iBuilder.UseMiddleware<CustomExceptionsCheckerMiddleware>();
    }
}
