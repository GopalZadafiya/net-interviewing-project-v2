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
            string message;
            var stackTrace = exception.StackTrace;

            switch (exception)
            {
                case ProductNotFoundException:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    _logger.LogInformation(exception, message);
                    break;

                case ProductTypeNotFoundException:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    _logger.LogWarning(exception, message);
                    break;

                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    _logger.LogWarning(exception, message);
                    break;

                case NotFoundException:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    _logger.LogWarning(exception, message);
                    break;

                case ArgumentNullException:
                    code = HttpStatusCode.BadRequest;
                    message = $"";
                    _logger.LogWarning(exception, message);
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    message = "Error occurred while processing your request";
                    _logger.LogError(exception, exception.Message);
                    break;
            }

            var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(exceptionResult);
        }
    }
}