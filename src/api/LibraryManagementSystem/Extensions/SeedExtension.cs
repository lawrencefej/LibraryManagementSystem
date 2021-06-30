using System.Threading.Tasks;
using LMSRepository.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Extensions
{
    public static class SeedExtension
    {
        public static async Task SeedData(this Seed seeder, DataContext dataContext)
        {
            await dataContext.Database.MigrateAsync();
            await seeder.SeedAuthors();
            await seeder.SeedUsers();
            await seeder.SeedLibraryCard();
        }
    }
}
