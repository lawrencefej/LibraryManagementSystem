using System;
using LMSEntities.Configuration;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBInit.Extensions
{
    public static class DataAccessConfiguration
    {
        public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            DbSettings dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();
            AwsSettings awsSettings = configuration.GetSection(nameof(AwsSettings)).Get<AwsSettings>();

            Console.Write(dbSettings.DatabaseName, "here");

            string connectionString = $"Server={dbSettings.Host};Port={dbSettings.Port};Database={dbSettings.DatabaseName};Uid={dbSettings.DbUser};Pwd={dbSettings.DbPassword};";

            // IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<DataContext>(x => x
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );
        }
    }
}
