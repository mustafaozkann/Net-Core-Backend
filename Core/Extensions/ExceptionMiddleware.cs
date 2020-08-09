using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";

            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            }

            if (e.GetType() == typeof(AuthorizeException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }

            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                Message = message,
                StatusCode = httpContext.Response.StatusCode
            }.ToString());
        }

        public class AuthorizeException : Exception
        {
            public AuthorizeException(string message) : base(message)
            {

            }
        }
    }
}
