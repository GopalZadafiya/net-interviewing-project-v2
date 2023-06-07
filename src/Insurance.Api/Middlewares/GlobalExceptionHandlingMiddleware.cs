using Insurance.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Insurance.Api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleAsync(httpContext, ex);
            }
        }

        private async Task HandleAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code;
            string message = exception.Message;
            var stackTrace = exception.StackTrace;
            var exceptionResult = JsonSerializer.Serialize(new { error = message });

            switch (exception)
            {
                case ProductNotFoundException:
                    code = HttpStatusCode.Conflict;
                    _logger.LogWarning(exception, message);
                    break;

                case ProductTypeNotFoundException:
                    code = HttpStatusCode.Conflict;
                    _logger.LogWarning(exception, message);
                    break;

                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    _logger.LogError(exception, message);
                    break;

                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    _logger.LogInformation(exception, message);
                    break;

                default:
                    code = HttpStatusCode.InternalServerError;
                    message = "Error occurred while processing your request";
                    exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });
                    _logger.LogError(exception, exception.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(exceptionResult);
        }
    }
}