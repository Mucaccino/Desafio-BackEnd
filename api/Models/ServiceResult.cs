namespace Motto.Models
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult<T> Successed(T data, int statusCode = 200)
        {
            return new ServiceResult<T> { Success = true, Data = data, StatusCode = statusCode };
        }

        public static ServiceResult<T> Successed(string message, int statusCode = 200)
        {
            return new ServiceResult<T> { Success = true, Message = message, StatusCode = statusCode };
        }

        public static ServiceResult<T> Failed(string message, int statusCode = 400)
        {
            return new ServiceResult<T> { Success = false, Message = message, StatusCode = statusCode };
        }
    }

}
