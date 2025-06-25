using Api.Errors;
using System.Net;

namespace Api.Middlewere
{
    public class ExceptionMiddlewere(IHostEnvironment environment, RequestDelegate request)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await request(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, environment);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment environment)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = environment.IsDevelopment()
                ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiErrorResponse(context.Response.StatusCode, "Internal Server Error");
           
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
