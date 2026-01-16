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

            var errorResponse = new ErrorResponseDto();

            if (exception is KeyNotFoundException)
            {
                errorResponse.StatusCode = 404;
                errorResponse.Message = exception.Message;
            }
            else if (exception is ArgumentException || exception is ArgumentNullException)
            {
                errorResponse.StatusCode = 400;
                errorResponse.Message = exception.Message;
            }
            else if (exception is DbUpdateException)
            {
                errorResponse.StatusCode = 400;
                errorResponse.Message = "Database error occurred";
                errorResponse.Details = exception.InnerException?.Message;
            }
            else
            {
                errorResponse.StatusCode = 500;
                errorResponse.Message = "An error occurred while processing your request";
                
                var env = context.RequestServices.GetService<IWebHostEnvironment>();
                if (env != null && env.IsDevelopment())
                {
                    errorResponse.Details = exception.ToString();
                }
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
