using System;
using LibraryManagementSystem.Helpers;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsDataAccessConfiguration
    {
        public static void AddDataAccessServices(this IServiceCollection services, AppSettings appSettings)
        {
            string connectionString = $"Server={appSettings.Host};Port={appSettings.Port};Database={appSettings.DatabaseName};Uid={appSettings.DbUser};Pwd={appSettings.DbPassword};";

            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<DataContext>(x => x
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );
        }
    }
}
