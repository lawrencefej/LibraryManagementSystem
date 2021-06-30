using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LMSService.Exceptions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected

            switch (exception)
            {
                case NoValuesFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case LMSValidationException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case LMSUnauthorizedException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case DbUpdateException:
                    code = HttpStatusCode.BadRequest;
                    break;
                default:
                    break;
            }

            string result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
