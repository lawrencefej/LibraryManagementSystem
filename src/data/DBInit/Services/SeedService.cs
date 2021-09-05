using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using DBInit.Interfaces;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;
using LMSRepository.Data;
using LMSRepository.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DBInit.Services
{
    public class SeedService : ISeedService
    {
        private readonly DataContext _context;

        // private readonly DataContext _context;
        // private readonly ILibraryCardService _libraryCardService;
        // private readonly IAdminService _adminService;
        // private readonly IAuthorService _authorService;
        // private readonly ICategoryService _categoryService;
        // private readonly ILibraryAssetService _libraryAssetService;

        // public SeedService(DataContext context, ILibraryCardService libraryCardService, IAdminService adminService, IAuthorService authorService, ICategoryService categoryService, ILibraryAssetService libraryAssetService)
        // {
        //     _context = context;
        //     _libraryCardService = libraryCardService;
        //     _adminService = adminService;
        //     _authorService = authorService;
        //     _categoryService = categoryService;
        //     _libraryAssetService = libraryAssetService;
        // }
        private readonly ILogger<SeedService> _logger;

        public SeedService(DataContext context, ILogger<SeedService> logger)
        {
            this._context = context;
            _logger = logger;
        }

        public async Task SeedDatabase()
        {
            // Order matters because data has to be created before lower data can be added
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.MigrateAsync();
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
            if (await _context.LibraryCards.AnyAsync())
            {
                return;
            }

            string libraryCardData = System.IO.File.ReadAllText("Data/LibraryCardData.json");
            List<LibraryCard> cards = JsonConvert.DeserializeObject<List<LibraryCard>>(libraryCardData);

            // foreach (LibraryCardForCreationDto card in cards)
            // {
            //     await _libraryCardService.AddLibraryCard(card);
            // }
            _context.AddRange(cards);
            await _context.SaveChangesAsync();
        }

        public async Task SeedUsers()
        {
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
        }

        public async Task SeedAuthors()
        {
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
        }

        public async Task SeedPastCheckout()
        {
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

        }

        public async Task SeedCurrentCheckout()
        {
            string checkoutData = System.IO.File.ReadAllText("Data/CheckoutCurrentSeedData.json");
            List<Checkout> checkouts = JsonConvert.DeserializeObject<List<Checkout>>(checkoutData);
            foreach (Checkout checkout in checkouts)
            {
                checkout.CheckoutDate = DateTime.Now.AddDays(GetRandomNumber(-6, 0));
            }

            _context.Checkouts.AddRange(checkouts);
            await _context.SaveChangesAsync();

        }

        public async Task SeedCategories()
        {
            if (await _context.Categories.AnyAsync())
            {
                return;
            }

            string authorData = System.IO.File.ReadAllText("Data/CategorySeedData.json");
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(authorData);

            // foreach (Category category in categories)
            // {
            //     await _categoryService.AddCategory(category);
            // }
            _context.AddRange(categories);
            await _context.SaveChangesAsync();
        }

        public async Task SeedBooksAsset()
        {
            if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Book))
            {
                string assetData = System.IO.File.ReadAllText("Data/BookSeedData.json");
                List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

                CleanAssetData(assets);
                // await _libraryAssetService.AddAsset(assets);
                _context.AddRange(assets);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedMediaAsset()
        {
            if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Media))
            {
                string assetData = System.IO.File.ReadAllText("Data/MediaSeedData.json");
                List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

                CleanAssetData(assets);
                // await _libraryAssetService.AddAsset(assets);
                _context.AddRange(assets);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SeedOtherAsset()
        {
            if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Other))
            {
                string assetData = System.IO.File.ReadAllText("Data/OtherSeedData.json");
                List<LibraryAsset> assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

                CleanAssetData(assets);
                // await _libraryAssetService.AddAsset(assets);
                _context.AddRange(assets);
                await _context.SaveChangesAsync();
            }
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
