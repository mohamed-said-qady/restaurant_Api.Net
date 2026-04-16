namespace restaurant.Helper
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        // النجاح: خليت الرسالة والكود اختياريين
        public static ServiceResult<T> Success(T data, string message = "Success", int statusCode = 200)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = statusCode,
                Message = message
            };
        }

        // الفشل: خليت الكود افتراضي 400 (Bad Request)
        public static ServiceResult<T> Failure(string message, int statusCode = 400)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Message = message
                
            };
        }
    }
}