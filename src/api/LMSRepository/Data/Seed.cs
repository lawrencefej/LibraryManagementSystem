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
                await _adminService.CreateUser(user);
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

        public async Task SeedCategories()
        {
            if (await _context.Categories.AnyAsync())
            {
                return;
            }

            string authorData = System.IO.File.ReadAllText("Data/CategorySeedData.json");
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(authorData);

            foreach (Category category in categories)
            {
                await _categoryService.AddCategory(category);
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
