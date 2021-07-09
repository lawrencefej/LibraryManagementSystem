using System.Threading.Tasks;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Extensions
{
    public static class SeedExtension
    {
        public static async Task SeedData(this Seed seeder, DataContext dataContext)
        {
            await dataContext.Database.EnsureDeletedAsync();
            await dataContext.Database.MigrateAsync();
            await seeder.SeedCategories();
            await seeder.SeedAuthors();
            await seeder.SeedUsers();
            await seeder.SeedLibraryCard();
            await seeder.SeedBooksAsset();
            await seeder.SeedMediaAsset();
            await seeder.SeedOtherAsset();
        }
    }
}
