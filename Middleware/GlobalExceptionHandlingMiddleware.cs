using System.Text.Json;
using JobApplicationTracker.WebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.WebApi.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Error: {Message}", ex.Message);
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ErrorResponseDto errorResponse;

            if (exception is KeyNotFoundException)
            {
                errorResponse = new ErrorResponseDto(
                    Message: exception.Message,
                    Details: null,
                    StatusCode: 404,
                    Timestamp: DateTime.UtcNow,
                    Errors: null
                );
            }
            else if (exception is ArgumentException || exception is ArgumentNullException)
            {
                errorResponse = new ErrorResponseDto(
                    Message: exception.Message,
                    Details: null,
                    StatusCode: 400,
                    Timestamp: DateTime.UtcNow,
                    Errors: null
                );
            }
            else if (exception is DbUpdateException)
            {
                errorResponse = new ErrorResponseDto(
                    Message: "Database error occurred",
                    Details: exception.InnerException?.Message,
                    StatusCode: 400,
                    Timestamp: DateTime.UtcNow,
                    Errors: null
                );
            }
            else
            {
                var env = context.RequestServices.GetService<IWebHostEnvironment>();
                errorResponse = new ErrorResponseDto(
                    Message: "An error occurred while processing your request",
                    Details: env != null && env.IsDevelopment() ? exception.ToString() : null,
                    StatusCode: 500,
                    Timestamp: DateTime.UtcNow,
                    Errors: null
                );
            }

            context.Response.StatusCode = errorResponse.StatusCode;

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            
            await context.Response.WriteAsync(json);
        }
    }
}
