using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;
using LMSRepository.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LMSRepository.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        private readonly ILibraryCardService _libraryCardService;
        private readonly IAdminService _adminService;
        private readonly IAuthorService _authorService;
        private readonly ICategoryService _categoryService;
        private readonly ILibraryAssetService _libraryAssetService;

        public Seed(DataContext context, ILibraryCardService libraryCardService, IAdminService adminService, IAuthorService authorService, ICategoryService categoryService, ILibraryAssetService libraryAssetService)
        {
            _libraryAssetService = libraryAssetService;
            _categoryService = categoryService;
            _authorService = authorService;
            _adminService = adminService;
            _libraryCardService = libraryCardService;
            _context = context;
        }

        public async Task SeedLibraryCard()
        {
            if (await _context.LibraryCards.AnyAsync())
            {
                return;
            }

            string libraryCardData = System.IO.File.ReadAllText("Data/LibraryCardData.json");
            List<LibraryCardForCreationDto> cards = JsonConvert.DeserializeObject<List<LibraryCardForCreationDto>>(libraryCardData);

            foreach (LibraryCardForCreationDto card in cards)
            {
                await _libraryCardService.AddLibraryCard(card);
            }
        }

        public async Task SeedUsers()
        {
            if (await _context.Users.Where(u => u.UserRoles.Any(r => r.Role.Name != nameof(EnumRoles.Member))).AnyAsync())
            {
                return;
            }

            string userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            List<AddAdminDto> users = JsonConvert.DeserializeObject<List<AddAdminDto>>(userData);

            foreach (AddAdminDto user in users)
            {
                await _adminService.CreateUser(user, true);
            }
        }

        public async Task SeedAuthors()
        {
            if (await _context.Authors.AnyAsync())
            {
                return;
            }

            string authorData = System.IO.File.ReadAllText("Data/AuthorSeedData.json");
            List<AuthorDto> authors = JsonConvert.DeserializeObject<List<AuthorDto>>(authorData);

            foreach (AuthorDto author in authors)
            {
                await _authorService.AddAuthor(author);
            }
        }

        public async Task SeedPastCheckout()
        {
            string checkoutData = System.IO.File.ReadAllText("Data/CheckoutPastSeedData.json");
            List<Checkout> checkouts = JsonConvert.DeserializeObject<List<Checkout>>(checkoutData);
            foreach (Checkout checkout in checkouts)
            {
                checkout.CheckoutDate = DateTime.UtcNow.AddDays(GetRandomNumber(-14, -7));
                checkout.DateReturned = DateTime.UtcNow.AddDays(GetRandomNumber(-6, 0));
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
                checkout.CheckoutDate = DateTime.UtcNow.AddDays(GetRandomNumber(-6, 0));
            }

            _context.Checkouts.AddRange(checkouts);
            await _context.SaveChangesAsync();

        }

        private static int GetRandomNumber(int start, int end)
        {
            return new Random().Next(start, end);
        }

        public async Task SeedCategories()
        {
            if (await _context.Categories.AnyAsync())
            {
                return;
            }

            string authorData = System.IO.File.ReadAllText("Data/CategorySeedData.json");
            List<CategoryDto> categories = JsonConvert.DeserializeObject<List<CategoryDto>>(authorData);

            foreach (CategoryDto category in categories)
            {
                await _categoryService.AddCategory(category);
            }
        }

        public async Task SeedBooksAsset()
        {
            if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Book))
            {
                string assetData = System.IO.File.ReadAllText("Data/BookSeedData.json");
                List<LibraryAssetForCreationDto> assets = JsonConvert.DeserializeObject<List<LibraryAssetForCreationDto>>(assetData);

                CleanAssetData(assets);
                await _libraryAssetService.AddAsset(assets);
            }
        }

        public async Task SeedMediaAsset()
        {
            if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Media))
            {
                string assetData = System.IO.File.ReadAllText("Data/MediaSeedData.json");
                List<LibraryAssetForCreationDto> assets = JsonConvert.DeserializeObject<List<LibraryAssetForCreationDto>>(assetData);

                CleanAssetData(assets);
                await _libraryAssetService.AddAsset(assets);
            }
        }

        public async Task SeedOtherAsset()
        {
            if (!_context.LibraryAssets.Any(i => i.AssetType == LibraryAssetType.Other))
            {
                string assetData = System.IO.File.ReadAllText("Data/OtherSeedData.json");
                List<LibraryAssetForCreationDto> assets = JsonConvert.DeserializeObject<List<LibraryAssetForCreationDto>>(assetData);

                CleanAssetData(assets);
                await _libraryAssetService.AddAsset(assets);
            }
        }

        private static void CleanAssetData(List<LibraryAssetForCreationDto> assets)
        {
            foreach (LibraryAssetForCreationDto asset in assets)
            {
                if (asset.AssetAuthors.Distinct().Count() > 1)
                {
                    List<LibraryAssetAuthorDto> AssetAuthors = new()
                    {
                        new LibraryAssetAuthorDto { AuthorId = new Random().Next(1, 5) },
                        new LibraryAssetAuthorDto { AuthorId = new Random().Next(6, 10) }
                    };
                    asset.AssetCategories.Clear();
                    asset.AssetAuthors = AssetAuthors;
                }

                if (asset.AssetCategories.Distinct().Count() > 1)
                {
                    List<LibraryAssetCategoryDto> AssetCategories = new()
                    {
                        new LibraryAssetCategoryDto { CategoryId = new Random().Next(1, 5) },
                        new LibraryAssetCategoryDto { CategoryId = new Random().Next(6, 10) }
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

        public async Task SeedAssets()
        {
            if (!_context.LibraryAssets.Any())
            {
                string assetData = System.IO.File.ReadAllText("Data/AssetSeedData.json");
                List<LibraryAssetForCreationDto> assets = JsonConvert.DeserializeObject<List<LibraryAssetForCreationDto>>(assetData);

                foreach (LibraryAssetForCreationDto asset in assets)
                {
                    await _libraryAssetService.AddAsset(asset);
                }
            }
        }
    }
}
