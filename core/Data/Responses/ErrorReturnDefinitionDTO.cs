namespace api.core.Data.Responses;

/// <summary>
/// A definition for an error returned to the front end.
/// [StatusCode] is the http error code
/// [ErrorCode] is a custom ERROR code that will be translated in the front end
/// [Message] a custom message defining the exception
/// </summary>
public class ErrorReturnDefinitionDTO
{
    public required int StatusCode { get; init; }
    public required string Code { get; init; }
    public required string Message { get; init; }
}