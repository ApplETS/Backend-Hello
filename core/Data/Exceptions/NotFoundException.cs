namespace api.core.Data.Exceptions;

/// <summary>
/// When an entity is not found
/// </summary>
/// <typeparam name="T">The entity type that wasn't found by the request</typeparam>
public class NotFoundException<T> : HttpException
{
    public NotFoundException()
        : base($"Object {typeof(T)?.Name ?? "UNKNOWN"} not found")
    {
        StatusCode = 404;
        ErrorCode = $"{typeof(T)?.Name?.ToUpper() ?? "UNKNOWN"}_NOT_FOUND";
    }


    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    public static void ThrowIfNull(T? argument)
    {
        if (argument is null)
            throw new NotFoundException<T>();
    }
}