namespace Motto.Domain.Services.Results;

/// <summary>
/// Represents the result of a service call.
/// </summary>
/// <typeparam name="T">The type of data returned by the service.</typeparam>
public class ServiceResult<T>
{
    /// <summary>
    /// Indicates whether the service call was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The message associated with the result.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The data returned by the service.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// The HTTP status code of the result.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Creates a successful result with the specified data and status code.
    /// </summary>
    public static ServiceResult<T> Successed(T data, int statusCode = 200)
    {
        return new ServiceResult<T> { Success = true, Data = data, StatusCode = statusCode };
    }

    /// <summary>
    /// Creates a successful result with the specified message and status code.
    /// </summary>
    public static ServiceResult<T> Successed(string message, int statusCode = 200)
    {
        return new ServiceResult<T> { Success = true, Message = message, StatusCode = statusCode };
    }

    /// <summary>
    /// Creates a failed result with the specified message and status code.
    /// </summary>
    public static ServiceResult<T> Failed(string message, int statusCode = 400)
    {
        return new ServiceResult<T> { Success = false, Message = message, StatusCode = statusCode };
    }
}
