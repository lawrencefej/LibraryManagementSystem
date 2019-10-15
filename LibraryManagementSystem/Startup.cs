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
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            CurrentEnv = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment CurrentEnv { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, SettingValidationStartupFilter>();

            var appSettingsSection = Configuration.GetSection(nameof(AppSettings));

            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            IdentityModelEventSource.ShowPII = true;
            services.AddDataAccessServices(appSettings.ConnectionString);
            services.AddIdentityConfiguration(appSettings.Token);
            services.AddMvcConfiguration();
            services.Configure<CloudinarySettings>(Configuration.GetSection(nameof(CloudinarySettings)));
            services.AddSingleton<ISmtpConfiguration>(Configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>());
            services.AddSingleton<IPhotoConfiguration>(Configuration.GetSection(nameof(CloudinarySettings)).Get<PhotoSettings>());
            services.AddThirdPartyConfiguration();
            services.AddCombinedInterfaces();

            if (CurrentEnv.IsProduction() || CurrentEnv.IsStaging())
            {
                services.AddProductionInterfaces();
            }
            else
            {
                services.AddDevelopmentInterfaces();
            }
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Integration"))
            {
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            loggerFactory.AddSerilog();
            // seeder.SeedUsers();
            // seeder.SeedAuthors();
            // seeder.SeedAssets();
            app.UseCors(x => x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            //.AllowCredentials());
            //app.UseCors(builder => builder.WithOrigins("http://localhost:4200"));
            //app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
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