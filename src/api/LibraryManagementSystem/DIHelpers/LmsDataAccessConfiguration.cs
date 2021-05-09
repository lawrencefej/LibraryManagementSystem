using LibraryManagementSystem.Helpers;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using System;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsDataAccessConfiguration
    {
        public static void AddDataAccessServices(this IServiceCollection services, AppSettings appSettings)
        {
            var connectionString = $"Server={appSettings.Host};Port={appSettings.Port};Database={appSettings.DatabaseName};Uid={appSettings.DbUser};Pwd={appSettings.DbPassword};";

            //var serverVersion = new MariaDbServerVersion(new Version());
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<DataContext>(x => x
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                //.ConfigureWarnings(t => t
                //.Ignore(CoreEventId.IncludeIgnoredWarning))
                );
        }
    }
}
