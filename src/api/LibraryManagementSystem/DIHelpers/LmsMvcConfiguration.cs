using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using LMSRepository.Interfaces.Helpers;
using LMSService.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsMvcConfiguration
    {
        public static void AddMvcConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });
            services.AddControllers(options =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(typeof(ValidateModelAttribute));
            })
            .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            // .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            // .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>());
            .AddFluentValidation(fv =>
            {
                // fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                fv.RegisterValidatorsFromAssemblyContaining<LibraryAssetValidator>();
            });
        }
    }
}
