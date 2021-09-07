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
            // Order matters because data has to be created before lower data can be added
            _logger.LogInformation("Deleting the database");
            await _context.Database.EnsureDeletedAsync();
            _logger.LogInformation("Database deleted successfully");

            _logger.LogInformation("Applying Migrations");
            await _context.Database.MigrateAsync();
            _logger.LogInformation("Migrations applied successfully");

            await SeedCategories();
            await SeedAuthors();
            await SeedUsers();
            await SeedLibraryCard();
            await SeedBooksAsset();
            await SeedMediaAsset();
            await SeedOtherAsset();
            await SeedPastCheckout();
            await SeedCurrentCheckout();
        }

        public async Task SeedLibraryCard()
        {
            _logger.LogInformation("Seeding LibraryCards");
            // if (await _context.LibraryCards.AnyAsync())
            // {
            //     return;
            // }

            string libraryCardData = System.IO.File.ReadAllText("Data/LibraryCardData.json");
            List<LibraryCard> cards = JsonConvert.DeserializeObject<List<LibraryCard>>(libraryCardData);

            foreach (LibraryCard card in cards)
            {
                // await _libraryCardService.AddLibraryCard(card);
                card.GenerateCardNumber();

                await GenerateCardNumber(cards, card);
            }

            _context.AddRange(cards);
            await _context.SaveChangesAsync();
            _logger.LogInformation("LibraryCards seeded successfully");
        }

        private async static Task GenerateCardNumber(List<LibraryCard> cards, LibraryCard card)
        {
            card.GenerateCardNumber();

            if (cards.Where(c => c != card).Any(x => x.CardNumber == card.CardNumber))
            {
                await Task.Delay(1000);
                card.GenerateCardNumber();

                await GenerateCardNumber(cards, card);
            }
        }

        public async Task SeedUsers()
        {
            _logger.LogInformation("Seeding Users");
            if (await _context.Users.Where(u => u.UserRoles.Any(r => r.Role.Name != nameof(EnumRoles.Member))).AnyAsync())
            {
                return;
            }

            string userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            List<AppUser> users = JsonConvert.DeserializeObject<List<AppUser>>(userData);

            // foreach (AddAdminDto user in users)
            // {
            //     await _adminService.CreateUser(user, true);
            // }
            // TODO check to see if roles are added
            _context.AddRange(users);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Users seeded successfully");
        }

        public async Task SeedAuthors()
        {
            _logger.LogInformation("Seeding Authors");
            // if (await _context.Authors.AnyAsync())
            // {
            //     return;
            // }

            string authorData = System.IO.File.ReadAllText("Data/AuthorSeedData.json");
            List<Author> authors = JsonConvert.DeserializeObject<List<Author>>(authorData);

            // foreach (AuthorDto author in authors)
            // {
            //     await _authorService.AddAuthor(author);
            // }

            _context.AddRange(authors);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Authors seeded successfully");
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
            // if (await _context.Categories.AnyAsync())
            // {
            //     return;
            // }

            string authorData = System.IO.File.ReadAllText("Data/CategorySeedData.json");
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(authorData);

            // foreach (Category category in categories)
            // {
            //     await _categoryService.AddCategory(category);
            // }
            _context.AddRange(categories);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Categories seeded successfully");
        }

        public async Task SeedBooksAsset()
        {
            _logger.LogInformation("Seeding Book Assets");
            // if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Book))
            // {
            string assetData = System.IO.File.ReadAllText("Data/BookSeedData.json");
            List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

            CleanAssetData(assets);
            // await _libraryAssetService.AddAsset(assets);
            _context.AddRange(assets);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Book Assets seeded successfully");
            // }
        }

        private async Task SeedMediaAsset()
        {
            _logger.LogInformation("Seeding media assets");
            // if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Media))
            // {
            string assetData = System.IO.File.ReadAllText("Data/MediaSeedData.json");
            List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

            CleanAssetData(assets);
            // await _libraryAssetService.AddAsset(assets);
            _context.AddRange(assets);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Media Assets seeded successfully");
            // }
        }

        private async Task SeedOtherAsset()
        {
            _logger.LogInformation("Seeding other assets");
            // if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Other))
            // {
            string assetData = System.IO.File.ReadAllText("Data/OtherSeedData.json");
            List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

            CleanAssetData(assets);

            _context.AddRange(assets);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Other Assets seeded successfully");
            // }
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

        // public async Task SeedAssets()
        // {
        //     // if (!_context.LibraryAssets.Any())
        //     // {
        //     string assetData = System.IO.File.ReadAllText("Data/AssetSeedData.json");
        //     List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

        //     // foreach (LibraryAssetForCreationDto asset in assets)
        //     // {
        //     //     await _libraryAssetService.AddAsset(asset);
        //     // }

        //     _context.AddRange(assets);
        //     await _context.SaveChangesAsync();
        //     // }
        // }
    }
}
