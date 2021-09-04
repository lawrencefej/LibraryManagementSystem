using System;
using System.Threading.Tasks;
using DBInit.Extensions;
using DBInit.Interfaces;
using DBInit.Services;
using LibraryManagementSystem.API.Helpers;
using LMSContracts.Interfaces;
using LMSService.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DBInit
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            using IHost host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
            )
            .ConfigureServices((context, services) =>
            {
                services.AddDataAccessServices(context.Configuration);
                services.AddTransient<ISeedService, SeedService>();
                services.AddTransient<IAdminService, AdminService>();
                services.AddTransient<IAuthorService, AuthorService>();
                services.AddTransient<ICategoryService, CategoryService>();
                services.AddTransient<ILibraryAssetService, LibraryAssetService>();
                services.AddTransient<ILibraryCardService, LibraryCardService>();
                services.AddTransient<ISeedService, SeedService>();
                services.AddAutoMapper(typeof(AutoMapperProfiles));
                services.AddHostedService<ApplicationStartup>();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddSystemsManager($"/lms/{context.HostingEnvironment.EnvironmentName}/", reloadAfter: TimeSpan.FromSeconds(20));
            });
    }
}
