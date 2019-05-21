using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LibraryManagementSystem.API
{
#pragma warning disable RCS1102 // Make class static.

    public class Program
    //#pragma warning restore RCS1102 // Make class static.
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    //logging.AddSerilog();
                })
                .UseStartup<Startup>();
    }
}