using System;
using DBInit.Interfaces;

namespace DBInit
{
    public class DBInitService : IDBInitService
    {
        private readonly ISeedService _seedService;

        public DBInitService(ISeedService seedService)
        {
            _seedService = seedService;
        }
        public async void Run()
        {
            Console.WriteLine("Hello World!");
            await _seedService.SeedLibraryCard();
        }
    }
}
