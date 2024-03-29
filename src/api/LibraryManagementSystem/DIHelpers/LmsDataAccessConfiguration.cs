﻿using LMSEntities.Configuration;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace LibraryManagementSystem.DIHelpers
{
    public static class LmsDataAccessConfiguration
    {
        public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            DbSettings dbSettings = configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();

            string connectionString = dbSettings.GetConnectionString();

            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<DataContext>(x => x
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                );
        }
    }
}
