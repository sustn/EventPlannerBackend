namespace EventPlanner.Application.Responses;
public class Response<T>
{
    public Response()
    {
        Success = true;
    }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string> Errors { get; set; } = null!;
    public T Result { get; set; } = default!;

    public static implicit operator Response<T>(string? v)
    {
        throw new NotImplementedException();
    }
}

public class Response
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = null!;
    public IEnumerable<string>? Errors { get; set; }
}
