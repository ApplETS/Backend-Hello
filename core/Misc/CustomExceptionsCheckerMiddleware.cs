using System.Text.Json;

using api.core.Data;
using api.core.Data.Exceptions;

namespace api.core.Misc;

public class CustomExceptionsCheckerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (HttpException e)
        {
            var body = new Response<ErrorReturnDefinitionDto>
            {
                Error = new ErrorReturnDefinitionDto
                {
                    StatusCode = e.StatusCode,
                    Code = e.ErrorCode,
                    Message = e.Message
                }
            };
            var jsonBody = JsonSerializer.Serialize(body);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = e.StatusCode;
            context.Response.ContentLength = jsonBody.Length;
            
            await context.Response.StartAsync();
            await context.Response.WriteAsync(jsonBody);
            await context.Response.CompleteAsync();
        }
    }
    
}