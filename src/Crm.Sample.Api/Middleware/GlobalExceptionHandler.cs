using Crm.Sample.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Crm.Sample.Api.Middleware
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception");

            var response = exception switch
            {
                DomainException => CreateProblemDetails(
                    HttpStatusCode.BadRequest,
                    exception.Message),

                _ => CreateProblemDetails(
                     HttpStatusCode.InternalServerError,
                     "Internal server error")
            };

            httpContext.Response.StatusCode = (int)response.Status;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(
                JsonSerializer.Serialize(response),
                cancellationToken);

            return true;
        }

        private static ProblemDetails CreateProblemDetails(
            HttpStatusCode status,
            string message)
        {
            return new ProblemDetails
            {
                Status = (int)status,
                Title = message
            };
        }
    }
}
