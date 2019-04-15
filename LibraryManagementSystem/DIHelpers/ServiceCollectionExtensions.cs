using AutoMapper;
using EmailService;
using EmailService.Services;
using LMSRepository.Interfaces.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace LibraryManagementSystem.DIHelpers
{
    public static class ServiceCollectionExtensions
    {
        public static void AddProductionInterfaces(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, SendGridService>();
        }

        public static void AddThirdPartyConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Library Management System", Version = "V1" });
            });
        }

        public static void AddDevelopmentInterfaces(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, MailtrapService>();
            services.AddTransient<Seed>();
        }
    }
}