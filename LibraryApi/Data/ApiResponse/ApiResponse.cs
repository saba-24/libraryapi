namespace LibraryApi.Data.ApiResponse;

public class ApiResponse
{
    private ApiResponse(bool isSuccess, string message, string? error, object data)
    {
        IsSuccess = isSuccess;
        Message = message;
        Error = error;
        Data = data;
    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public string? Error { get; set; }
    public object? Data { get; set; }

    public static ApiResponse Success(string message, object data)
    {
        return new ApiResponse(true, message, null, data);
    }

    public static ApiResponse Fail(string message, string error)
    {
        return new ApiResponse(false, message, error, null);
    }
}