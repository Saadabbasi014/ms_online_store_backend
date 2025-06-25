namespace Api.Errors
{
    public class ApiErrorResponse(int statusCode, string message, string? detail = null)
    {
        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message;
        public string? Detail { get; set; } = detail;
    }
}
