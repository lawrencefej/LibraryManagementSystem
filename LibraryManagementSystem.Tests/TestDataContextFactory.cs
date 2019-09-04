using LMSRepository.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryManagementSystem.Tests
{
    public class TestDataContextFactory
    {
        private readonly DbContextOptions<DataContext> _options;

        public DataContext UseInMemory() => new DataContext(_options);

        public DataContext UseSqlite()
        {
            useSqlite = true;
            return new DataContext(_options);
        }

        private bool useSqlite;

        public TestDataContextFactory()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            if (useSqlite)
            {
                // Use Sqlite DB.
                var connection = new SqliteConnection("DataSource=:memory:");
                builder.UseSqlite(connection);
            }
            else
            {
                // Use In-Memory DB.
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }

            using (var ctx = new DataContext(builder.Options))
            {
                ctx.Database.EnsureCreated();
            }

            _options = builder.Options;
        }
    }
}