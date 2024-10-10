using System.Net;

namespace commentsAPI.Entities.Shared
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ApiResponse() { }

        protected ApiResponse(bool isSuccess, string? message, HttpStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
        }

        public static ApiResponse Succeed(HttpStatusCode statusCode, string? message = null)
        {
            return new ApiResponse(true, message, statusCode);
        }

        public static ApiResponse Fail(string message, HttpStatusCode statusCode)
        {
            return new ApiResponse(false, message, statusCode);
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Result { get; set; }

        private ApiResponse(bool isSuccess, string? message, HttpStatusCode statusCode, T? result) : base(isSuccess, message, statusCode)
        {
            Result = result;
        }

        public static ApiResponse<T> Succeed(HttpStatusCode statusCode, T? result, string? message = null)
        {
            return new ApiResponse<T>(true, message, statusCode, result);
        }

        public static new ApiResponse<T> Fail(string? message, HttpStatusCode statusCode)
        {
            return new ApiResponse<T>(false, message, statusCode, default);
        }
    }
}
