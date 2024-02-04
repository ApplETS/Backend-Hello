namespace api.core.Data.Exceptions;

/// <summary>
/// When an entity is not found
/// </summary>
/// <typeparam name="T">The entity type that wasn't found by the request</typeparam>
public class BadParameterException<T> : HttpException
{
    public BadParameterException(string fieldName, string fieldParsingErrorMessage)
        : base($"[{fieldName}] {fieldParsingErrorMessage}")
    {
        StatusCode = 400;
        ErrorCode = $"BAD_PARAM_{nameof(T).ToUpper()}_{fieldName}";
    }
}
