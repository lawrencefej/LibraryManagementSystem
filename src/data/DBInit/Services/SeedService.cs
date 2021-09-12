using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBInit.Interfaces;
using LMSEntities.Models;
using LMSRepository.Data;
using LMSRepository.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DBInit.Services
{
    public class SeedService : ISeedService
    {
        private readonly DataContext _context;
        private readonly ILogger<SeedService> _logger;

        public SeedService(IServiceProvider serviceProvider, ILogger<SeedService> logger)
        {
            _context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
            _logger = logger;
        }

        public async Task SeedDatabase()
        {
            _logger.LogInformation("Deleting the database");
            await _context.Database.EnsureDeletedAsync();
            _logger.LogInformation("Database deleted successfully");

            _logger.LogInformation("Applying Migrations");
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Migrations applied successfully");

            // Order matters because data has to be created before lower data can be added
            await SeedCategories();
            await SeedAuthors();
            await SeedUsers();
            await SeedLibraryCard();
            await SeedBooksAsset();
            await SeedMediaAsset();
            await SeedOtherAsset();
            await SeedPastYearCheckout();
            await SeedPastCheckout();
            await SeedCurrentCheckout();
        }

        public async Task SeedLibraryCard()
        {
            _logger.LogInformation("Seeding LibraryCards");

            string libraryCardData = System.IO.File.ReadAllText("Data/LibraryCardData.jsonc");
            List<LibraryCard> cards = JsonConvert.DeserializeObject<List<LibraryCard>>(libraryCardData);

            _context.AddRange(cards);
            await _context.SaveChangesAsync();
            _logger.LogInformation("LibraryCards seeded successfully");
        }

        public async Task SeedUsers()
        {
            _logger.LogInformation("Seeding Users");
            if (await _context.Users.Where(u => u.UserRoles.Any(r => r.Role.Name != nameof(EnumRoles.Member))).AnyAsync())
            {
                return;
            }

            string userData = System.IO.File.ReadAllText("Data/UserSeedData.jsonc");
            List<AppUser> users = JsonConvert.DeserializeObject<List<AppUser>>(userData);

            foreach (AppUser user in users)
            {
                user.Created = DateTime.UtcNow;
            }
            _context.AddRange(users);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Users seeded successfully");
        }

        public async Task SeedAuthors()
        {
            _logger.LogInformation("Seeding Authors");

            string authorData = System.IO.File.ReadAllText("Data/AuthorSeedData.json");
            List<Author> authors = JsonConvert.DeserializeObject<List<Author>>(authorData);

            _context.AddRange(authors);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Authors seeded successfully");
        }

        public async Task SeedPastYearCheckout()
        {
            _logger.LogInformation("Seeding Past Year Checkouts");
            string checkoutData = System.IO.File.ReadAllText("Data/CheckoutPastYearSeedData.json");
            List<Checkout> checkouts = JsonConvert.DeserializeObject<List<Checkout>>(checkoutData);

            int pastMonth = 12;

            for (int i = 1; i < pastMonth; i++)
            {
                List<Checkout> monthsCheckouts = checkouts.OrderBy(x => new Random().Next())
                    .Take(GetRandomNumber(1000, 4000))
                    .Select(item => new Checkout { LibraryAssetId = item.LibraryAssetId, LibraryCardId = item.LibraryCardId })
                    .ToList();
                await SeedMonthData(monthsCheckouts, i);
            }

            _logger.LogInformation("Past year checkouts seeded successfully");

        }

        private async Task SeedMonthData(List<Checkout> checkouts, int periodPast)
        {
            DateTime month = DateTime.UtcNow.AddMonths(-periodPast);
            string monthName = month.ToString("MMMM");
            _logger.LogInformation("Seeding checkouts for {monthName}", monthName);

            foreach (Checkout checkout in checkouts)
            {
                DateTime checkouDate = month.AddDays(GetRandomNumber(-14, -7));
                checkout.CheckoutDate = checkouDate;
                checkout.DueDate = checkouDate.AddDays(21);
                checkout.DateReturned = checkouDate.AddDays(GetRandomNumber(-6, 0));
                checkout.Status = CheckoutStatus.Returned;
            }

            _context.Checkouts.AddRange(checkouts);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded checkouts for {monthName} successfully", monthName);
        }

        public async Task SeedPastCheckout()
        {
            _logger.LogInformation("Seeding Past checkouts");
            string checkoutData = System.IO.File.ReadAllText("Data/CheckoutPastSeedData.json");
            List<Checkout> checkouts = JsonConvert.DeserializeObject<List<Checkout>>(checkoutData);

            foreach (Checkout checkout in checkouts)
            {
                checkout.CheckoutDate = DateTime.Now.AddDays(GetRandomNumber(-14, -7));
                checkout.DateReturned = DateTime.Now.AddDays(GetRandomNumber(-6, 0));
                checkout.Status = CheckoutStatus.Returned;
            }

            _context.Checkouts.AddRange(checkouts);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Past checkouts seeded successfully");

        }

        public async Task SeedCurrentCheckout()
        {
            _logger.LogInformation("Seeding Current checkouts");
            string checkoutData = System.IO.File.ReadAllText("Data/CheckoutCurrentSeedData.json");
            List<Checkout> checkouts = JsonConvert.DeserializeObject<List<Checkout>>(checkoutData);

            foreach (Checkout checkout in checkouts)
            {
                checkout.CheckoutDate = DateTime.Now.AddDays(GetRandomNumber(-6, 0));
            }

            _context.Checkouts.AddRange(checkouts);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Current checkouts seeded successfully");
        }

        public async Task SeedCategories()
        {
            _logger.LogInformation("Seeding Categories");

            string authorData = System.IO.File.ReadAllText("Data/CategorySeedData.json");
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(authorData);

            _context.AddRange(categories);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Categories seeded successfully");
        }

        public async Task SeedBooksAsset()
        {
            _logger.LogInformation("Seeding Book Assets");
            string assetData = System.IO.File.ReadAllText("Data/BookSeedData.json");
            List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

            CleanAssetData(assets);
            _context.AddRange(assets);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Book Assets seeded successfully");
        }

        private async Task SeedMediaAsset()
        {
            _logger.LogInformation("Seeding media assets");
            string assetData = System.IO.File.ReadAllText("Data/MediaSeedData.json");
            List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

            CleanAssetData(assets);
            _context.AddRange(assets);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Media Assets seeded successfully");
        }

        private async Task SeedOtherAsset()
        {
            _logger.LogInformation("Seeding other assets");
            string assetData = System.IO.File.ReadAllText("Data/OtherSeedData.json");
            List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

            CleanAssetData(assets);

            _context.AddRange(assets);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Other Assets seeded successfully");
        }

        private static void CleanAssetData(List<LibraryAsset> assets)
        {
            foreach (LibraryAsset asset in assets)
            {
                if (asset.AssetAuthors.Distinct().Count() > 1)
                {
                    List<LibraryAssetAuthor> AssetAuthors = new()
                    {
                        new LibraryAssetAuthor { AuthorId = new Random().Next(1, 5) },
                        new LibraryAssetAuthor { AuthorId = new Random().Next(6, 10) }
                    };
                    asset.AssetCategories.Clear();
                    asset.AssetAuthors = AssetAuthors;
                }

                if (asset.AssetCategories.Distinct().Count() > 1)
                {
                    List<LibraryAssetCategory> AssetCategories = new()
                    {
                        new LibraryAssetCategory { CategoryId = new Random().Next(1, 5) },
                        new LibraryAssetCategory { CategoryId = new Random().Next(6, 10) }
                    };
                    asset.AssetCategories.Clear();
                    asset.AssetCategories = AssetCategories;
                }

                if (asset.Description.Length > 250)
                {
                    asset.Description = asset.Description.Substring(0, 250);
                }

                if (asset.Title.Length > 50)
                {
                    asset.Title = asset.Title.Substring(0, 50);
                }
            }
        }

        private static int GetRandomNumber(int start, int end)
        {
            return new Random().Next(start, end);
        }
    }
}
