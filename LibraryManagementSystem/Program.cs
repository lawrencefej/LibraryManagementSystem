using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
                .UseStartup<Startup>();
    }
}