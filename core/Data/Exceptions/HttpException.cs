namespace api.core.Data.Exceptions;

public abstract class HttpException : Exception
{
    public int StatusCode { get; protected set; }

    public string ErrorCode { get; protected set; }
    
    public HttpException()
    {
    }

    public HttpException(string message)
        : base(message)
    {
    }

    public HttpException(string message, Exception inner)
        : base(message, inner)
    {
    }
}