using AutoMapper;
using EmailService;
using EmailService.Services;
using LibraryManagementSystem.API.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PhotoLibrary;
using PhotoLibrary.Service;
using Swashbuckle.AspNetCore.Swagger;

namespace LibraryManagementSystem.DIHelpers
{
    public static class ServiceCollectionExtensions
    {
        public static void AddProductionInterfaces(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, SendGridService>();
            services.AddScoped<IPhotoLibraryService, CloudinaryService>();
        }

        public static void AddThirdPartyConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Management System", Version = "V1" });
            });
        }

        public static void AddDevelopmentInterfaces(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, MailtrapService>();
            services.AddScoped<IPhotoLibraryService, CloudinaryService>();
            // services.AddTransient<Seed>();
        }
    }
}