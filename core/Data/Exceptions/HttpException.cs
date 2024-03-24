namespace api.core.Data.Exceptions;

public abstract class HttpException : Exception
{
    public int StatusCode { get; protected set; }

    public string ErrorCode { get; protected set; }
    
    public HttpException()
    {
        StatusCode = 500;
        ErrorCode = "INTERNAL_SERVER_ERROR";
    }

    public HttpException(string message)
        : base(message)
    {
        StatusCode = 500;
        ErrorCode = "INTERNAL_SERVER_ERROR";
    }

    public HttpException(string message, Exception inner)
        : base(message, inner)
    {
        StatusCode = 500;
        ErrorCode = "INTERNAL_SERVER_ERROR";
    }
}