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

            string connectionString = dbSettings.GetConnectionString();

            services.AddDbContext<DataContext>(option => option
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );
        }
    }
}
