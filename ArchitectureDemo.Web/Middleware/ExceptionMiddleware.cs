using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog.Core;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace ArchitectureDemo.Web.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        private readonly ILogger<ExceptionMiddleware> logger = logger;
        private readonly RequestDelegate next = next;
        private readonly IHostEnvironment env = env;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            var requestDetails = new
            {
                TraceId = traceId,
                RequestMethod = context.Request.Method,
                RequestPath = context.Request.Path,
                QueryString = context.Request.QueryString.ToString(),
                ClientIp = context.Connection.RemoteIpAddress?.ToString(),
                UserName = context.User?.Identity?.Name ?? "Anonymous"
            };

            logger.LogError(exception, "Unhandled exception occurred. Details: {@RequestDetails}", requestDetails);

            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path
            };

            switch (exception)
            {
                case KeyNotFoundException e:
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                    problemDetails.Title = "The specified resource was not found.";
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Detail = e.Message;
                    break;
                default:
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
                    problemDetails.Title = "An internal server error has occurred.";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = env.IsDevelopment()
                        ? $"{exception.Message}\n{exception.StackTrace}"
                        : "An unexpected error occurred. Please contact support and provide the TraceId.";
                    break;
            }

            problemDetails.Extensions["traceId"] = traceId;
            context.Response.StatusCode = problemDetails.Status.Value;

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }
    }
}
