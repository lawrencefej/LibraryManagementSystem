using LibraryManagementSystem.API.Helpers;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace LibraryManagementSystem.DIHelpers
{
    public static class ServiceCollectionExtensions
    {
        public static void AddProductionInterfaces(this IServiceCollection services)
        {
            // services.AddScoped<IEmailService, SendGridService>();
        }

        public static void AddThirdPartyConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Management System", Version = "V1" });
            });
            services.AddFluentValidationRulesToSwagger();
        }

        public static void AddDevelopmentInterfaces(this IServiceCollection services)
        {
            // services.AddScoped<IEmailService, MailtrapService>();
        }
    }
}
