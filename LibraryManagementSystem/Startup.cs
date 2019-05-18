using EmailService.Configuration;
using LibraryManagementSystem.DIHelpers;
using LibraryManagementSystem.Helpers;
using LMSRepository.Interfaces.Helpers;
using LMSService.Exceptions;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using PhotoLibrary.Configuration;
using Serilog;

namespace LibraryManagementSystem.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            IdentityModelEventSource.ShowPII = true;
            services.AddDataAccessServices(appSettings.ConnectionString);
            services.AddIdentityConfiguration(appSettings.Token);
            services.AddMvcConfiguration();
            services.Configure<CloudinarySettings>(Configuration.GetSection(nameof(CloudinarySettings)));
            services.AddSingleton<ISmtpConfiguration>(Configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>());
            services.AddSingleton<IPhotoConfiguration>(Configuration.GetSection(nameof(CloudinarySettings)).Get<PhotoSettings>());
            services.AddThirdPartyConfiguration();

            services.AddCombinedInterfaces();
            services.AddProductionInterfaces();
            services.AddOData();
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            IdentityModelEventSource.ShowPII = true;
            services.AddDataAccessServices(appSettings.ConnectionString);
            services.AddIdentityConfiguration(appSettings.Token);
            services.AddMvcConfiguration();
            services.Configure<CloudinarySettings>(Configuration.GetSection(nameof(CloudinarySettings)));
            services.AddSingleton<ISmtpConfiguration>(Configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>());
            services.AddSingleton<IPhotoConfiguration>(Configuration.GetSection(nameof(CloudinarySettings)).Get<PhotoSettings>());
            services.AddThirdPartyConfiguration();

            services.AddCombinedInterfaces();
            services.AddDevelopmentInterfaces();
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
                //app.UseDeveloperExceptionPage();
                //app.ConfigureCustomExceptionMiddleware();
            }
            else
            {
                //app.ConfigureCustomExceptionMiddleware();
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
                //app.UseExceptionHandler(builder => {
                //    builder.Run(async context => {
                //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //        var error = context.Features.Get<IExceptionHandlerFeature>();
                //        if (error != null)
                //        {
                //            context.Response.AddApplicationError(error.Error.Message);
                //            await context.Response.WriteAsync(error.Error.Message);
                //        }

                //    });
                //});
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            loggerFactory.AddSerilog();
            // seeder.SeedUsers();
            // seeder.SeedAuthors();
            // seeder.SeedAssets();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseMvc();
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Fallback", action = "Index" }
                    );
                routeBuilder.EnableDependencyInjection();
                routeBuilder.Expand().Select().Count().OrderBy().Filter().MaxTop(null);
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management System V1");
            });
        }
    }
}