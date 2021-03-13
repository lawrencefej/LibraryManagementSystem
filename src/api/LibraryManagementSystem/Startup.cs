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
using Microsoft.Extensions.Hosting;
using PhotoLibrary.Configuration;
using Serilog;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            CurrentEnv = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment CurrentEnv { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, SettingValidationStartupFilter>();

            var appSettingsSection = Configuration.GetSection(nameof(AppSettings));

            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            //IdentityModelEventSource.ShowPII = true;
            services.AddDataAccessServices(appSettings);
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

            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "wwwroot";
                // config.RootPath = "ClientApp/dist";
            });
            //services.AddOData();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, DataContext dataContext)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Integration"))
            {
                dataContext.Database.Migrate();
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware(typeof(ErrorHandlingMiddleware));

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            loggerFactory.AddSerilog();
            // seeder.SeedUsers();
            // seeder.SeedAuthors();
            // seeder.SeedAssets();
            app.UseRouting();
            // TODO Confirm if Cors in needed since we are now using spa proxy
            //app.UseCors(x => x.AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());
            // app.UseCors("CorsPolicy");
            //.AllowCredentials());
            //app.UseCors(builder => builder.WithOrigins("http://localhost:4200"));
            app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            // app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management System V1");
            });
            //app.UseMvc(routeBuilder =>
            //{
            //    routeBuilder.MapSpaFallbackRoute(
            //        name: "spa-fallback",
            //        defaults: new { controller = "Fallback", action = "Index" }
            //        );
            //    routeBuilder.EnableDependencyInjection();
            // TODO Fix or Remove OData
            //    routeBuilder.Expand().Select().Count().OrderBy().Filter().MaxTop(null);
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapFallbackToController("Index", "Fallback");
                endpoints.MapFallbackToFile("index.html");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../../spa/";
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
