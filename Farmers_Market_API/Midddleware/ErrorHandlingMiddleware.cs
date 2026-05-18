// Week 4 - Wed 18 May: Global exception handling middleware

using System.Net;
using System.Text.Json;
using FarmerMarketAPI.Exceptions;

namespace FarmerMarketAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
            
            var response = context.Response;
            response.ContentType = "application/json";
            
            var errorResponse = new
            {
                error = exception.Message,
                type = exception.GetType().Name,
                timestamp = DateTime.UtcNow,
                path = context.Request.Path
            };
            
            switch (exception)
            {
                case ListingNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                    
                case InvalidOperationException:
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;
                    
                case ArgumentException:
                case InvalidDataException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                    
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = new 
                    { 
                        error = "An internal server error occurred. Please try again later.",
                        type = "InternalServerError",
                        timestamp = DateTime.UtcNow,
                        path = context.Request.Path
                    };
                    break;
            }
            
            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(jsonResponse);
        }
    }
}