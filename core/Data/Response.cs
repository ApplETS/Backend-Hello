namespace api.core.Data;

/// <summary>
/// A common data response to return to the front end. Data or Error should be set but not
/// at the same time.
/// </summary>
/// <typeparam name="T">The object you want to return</typeparam>
public class Response<T>
{
    public T? Data { get; init; }
    public T? Error { get; init; }
}
