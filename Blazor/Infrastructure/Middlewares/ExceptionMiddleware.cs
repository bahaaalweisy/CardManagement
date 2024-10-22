using CardManagement.Core.Models.Common;

using System.Net;
using System.Text.Json;

namespace CardManagementApis.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        /// <summary>
        /// The _next.
        /// </summary>
        private readonly RequestDelegate _next;

        private readonly ILoggerFactory _loggerFactory;
        /// <summary>
        /// the error
        /// </summary>
        private ReturnValuedResult<List<string>> _error;


        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalizationMiddleware"/> class.
        /// </summary>
        /// <param name="next">
        /// The next.
        /// </param>
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// The invoke.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
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
            var error = new ReturnValuedResult<List<string>>();
            context.Response.ContentType = "application/json";
            var _logger = _loggerFactory.CreateLogger<ExceptionMiddleware>();

            if (exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                error.Errors.Add(exception.Message);
            }
            else
            {
                _logger.LogError(exception, "{Message}{LogLevel}{TimeStamp}{Exception}{MachineName}{HttpVerb}{RequestHost}{Url}{LoggerType}{Date}", exception.Message, LogLevel.Error, DateTime.Now, exception, null, context.Request.Method, context.Request.Host.Value, context.Request.Path.Value, null, DateTime.Now);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            }
            error.Errors.Add(exception.Message);
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(error, options));
        }
    }
}
