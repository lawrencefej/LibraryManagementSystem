using Microsoft.AspNetCore.Builder;

namespace LMSService.ErrorHandling
{
    public static class ConfigureCustomExceptionMiddleware
    {
        public static void ConfigureCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}