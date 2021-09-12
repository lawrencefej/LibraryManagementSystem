using LibraryManagementSystem.DIHelpers;
using LibraryManagementSystem.Extensions;
using LibraryManagementSystem.Helpers;
using LMSEntities.Configuration;
using LMSRepository.Data;
using LMSService.Exceptions;
using LMSService.Helpers;
using LMSService.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

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
            // TODO validate configs
            services.AddTransient<IStartupFilter, SettingValidationStartupFilter>();

            //IdentityModelEventSource.ShowPII = true;
            services.AddDataAccessServices(Configuration);
            services.AddIdentityConfiguration(Configuration);
            services.AddMvcConfiguration();
            services.Configure<AwsSettings>(Configuration.GetSection(nameof(AwsSettings)));
            services.Configure<JwtSettings>(Configuration.GetSection(nameof(JwtSettings)));
            services.Configure<SmtpSettings>(Configuration.GetSection(nameof(SmtpSettings)));
            services.Configure<DbSettings>(Configuration.GetSection(nameof(DbSettings)));
            services.Configure<CloudinarySettings>(Configuration.GetSection(nameof(CloudinarySettings)));
            services.AddThirdPartyConfiguration();
            services.AddCombinedInterfaces();
            services.AddSignalR(e => e.EnableDetailedErrors = true);

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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
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
            app.UseRouting();
            app.UseCors("CorsPolicy");
            //.AllowCredentials());
            //app.UseCors(builder => builder.WithOrigins("http://localhost:4200"));
            app.UseHttpsRedirection();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
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
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DashboardHub>("/hubs/chart");
                if (!env.IsDevelopment())
                {
                    endpoints.MapFallbackToController("Index", "Fallback");
                    // endpoints.MapFallbackToFile("index.html");
                }
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
                else
                {
                    spa.Options.SourcePath = "../../spa/";

                }
            });
        }
    }
}
