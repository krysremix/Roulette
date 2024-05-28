using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System;
using Infrastructure.Exceptions;
using System.Text.Json;

namespace Roulette.MiddleWare
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var isHandledError = false;

                switch (error)
                {
                    case AppException ex:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        isHandledError = true;
                        _logger.LogError(ex.Message, ex);
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logger.LogError(error.Message, error);
                        break;
                }

                var result = JsonSerializer.Serialize(new { Error = error?.Message, isHandledError });
                await response.WriteAsync(result);
            }
        }
    }
}
