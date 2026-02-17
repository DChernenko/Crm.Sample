using Crm.Sample.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Crm.Sample.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
            _logger.LogError(exception, "An unhandled exception occurred.");

            var response = context.Response;
            response.ContentType = "application/json";

            var (statusCode, message) = GetStatusCodeAndMessage(exception);

            response.StatusCode = (int)statusCode;

            var problemDetails = new
            {
                Status = (int)statusCode,
                Title = message,
                Detail = _env.IsDevelopment() ? exception.StackTrace : null,
                Instance = context.Request.Path
            };

            var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await response.WriteAsync(json);
        }

        private static (HttpStatusCode StatusCode, string Message) GetStatusCodeAndMessage(Exception exception)
        => exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ValidationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An internal server error occurred. Please try again later.")
        };
    }
}
