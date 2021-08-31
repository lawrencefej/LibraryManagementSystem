using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // TODO figure out the reload time
                config.AddSystemsManager($"/lms/{context.HostingEnvironment.EnvironmentName}/", reloadAfter: TimeSpan.FromSeconds(20));
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });
        //.ConfigureLogging(logging =>
        //{
        //    logging.ClearProviders();
        //})
        //.UseStartup<Startup>();
        // Todo Fix logging
    }
}
