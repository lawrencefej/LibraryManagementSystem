using System;

namespace LibraryManagementSystem.Tests.Infrastructure
{
    public class LMSTestBase : IDisposable
    {
        //public LMSTestBase()
        //{
        //    var options = new DbContextOptionsBuilder<DbContext>()
        //        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        //        .Options;

        //    _context = new DbContext(options);

        //    _context.Database.EnsureCreated();

        //    NorthwindInitializer.Initialize(_context);
        //}

        public void Dispose()
        {
            //_context.Database.EnsureDeleted();

            //_context.Dispose();
        }

        public void Setup()
        {
        }
    }
}