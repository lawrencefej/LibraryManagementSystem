using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsDataAccessConfiguration
    {
        public static void AddDataAccessServices(this IServiceCollection services, string connectionString)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<DataContext>(x => x
                .UseMySql(connectionString)
                //.ConfigureWarnings(t => t
                //.Ignore(CoreEventId.IncludeIgnoredWarning))
                );
        }
    }
}