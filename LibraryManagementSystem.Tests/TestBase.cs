using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryManagementSystem.Tests
{
    public class TestBase
    {
        private bool useSqlite;

        public DataContext GetDbContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            if (useSqlite)
            {
                // Use Sqlite DB.
                builder.UseSqlite("DataSource=:memory:", x => { });
            }
            else
            {
                // Use In-Memory DB.
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            }

            var DataContext = new DataContext(builder.Options);
            if (useSqlite)
            {
                // SQLite needs to open connection to the DB.
                // Not required for in-memory-database and MS SQL.
                DataContext.Database.OpenConnection();
            }

            DataContext.Database.EnsureCreated();

            return DataContext;
        }

        public void UseSqlite()
        {
            useSqlite = true;
        }
    }
}