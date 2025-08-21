using System.Net;
using System.Text.Json;
using FluentValidation;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.API
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                TaskNotFoundException => new ErrorResponse
                {
                    Title = "Task Not Found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = exception.Message
                },
                ValidationException validationEx => new ErrorResponse
                {
                    Title = "Validation Failed",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = "One or more validation errors occurred",
                    Errors = validationEx.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage)
                },
                ArgumentException => new ErrorResponse
                {
                    Title = "Bad Request",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = exception.Message
                },
                _ => new ErrorResponse
                {
                    Title = "Internal Server Error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "An unexpected error occurred"
                }
            };

            context.Response.StatusCode = response.Status;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class ErrorResponse
    {
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Detail { get; set; } = string.Empty;
        public Dictionary<string, string>? Errors { get; set; }
    }
}
