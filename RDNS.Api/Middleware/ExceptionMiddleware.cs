using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SubModels.Utilities;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace RDNS.Api.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            //Default values
            var statusCode = (int)HttpStatusCode.InternalServerError;
            object message = "Unexpected Error";

            if (exception is HttpResponseException)
            {
                if (exception is HttpResponseException customException)
                {
                    message = customException.Content;
                    statusCode = customException.StatusCode;
                }
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new
            {
                HttpStatusCode = statusCode,
                IsSuccess = false,
                Message = message,
                MessageTypeCode = "error",
                RequestLinkHref = context.Request.Path,
                RequestMethod = context.Request.Method
            });

            return context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
