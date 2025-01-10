namespace Auth.Core.CQRS;

public class Result<T>
{
    public T Data { get; private set; } = default!;
    
    public ErrorResponse? ErrorResponse { get; private set; }
    
    public bool IsSuccess => ErrorResponse is null;

    public static Result<T> Success(T data) => new Result<T> {Data = data};

    public static Result<T> Error(ErrorResponse error) => new Result<T> {ErrorResponse = error};
    
    public static implicit operator Result<T>(ErrorResponse errorResponse)
    {
        return new Result<T> {ErrorResponse = errorResponse};
    }

    public static implicit operator Result<T>(T data)
    {
        return new Result<T> {Data = data};
    }

    private Result() {}
}

public abstract class ErrorResponse
{
    public string Message { get; }

    protected ErrorResponse(string message)
    {
        Message = message;
    }
}