using LMSRepository.Data;
using LMSRepository.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LMSRepository.Interfaces.DataAccess
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly DataContext _context;

        public Seed(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    user.UserName = user.Email;
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Member").Wait();
                }

                var adminUser = new User
                {
                    Email = "a@x.com",
                };

                adminUser.UserName = adminUser.Email;

                var librarianUser = new User
                {
                    Email = "b@x.com"
                };

                librarianUser.UserName = librarianUser.Email;

                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;
                IdentityResult result2 = _userManager.CreateAsync(librarianUser, "password").Result;

                if (result.Succeeded && result2.Succeeded)
                {
                    var admin = _userManager.FindByEmailAsync("a@x.com").Result;
                    _userManager.AddToRoleAsync(admin, "Admin").Wait();

                    var librarian = _userManager.FindByEmailAsync("b@x.com").Result;
                    _userManager.AddToRoleAsync(librarian, "Librarian").Wait();
                }
            }
        }

        public void SeedAuthors()
        {
            if (!_context.Authors.Any())
            {
                var authorData = System.IO.File.ReadAllText("AuthorSeedData.json");
                var authors = JsonConvert.DeserializeObject<List<Author>>(authorData);

                foreach (var author in authors)
                {
                    _context.Add(author);
                }

                _context.SaveChanges();
            }
        }

        public void SeedAssets()
        {
            if (!_context.LibraryAssets.Any())
            {
                var assetData = System.IO.File.ReadAllText("AssetSeedData.json");
                var assets = JsonConvert.DeserializeObject<List<LibraryAsset>>(assetData);

                foreach (var asset in assets)
                {
                    asset.CopiesAvailable = asset.NumberOfCopies;
                    _context.Add(asset);
                }

                _context.SaveChanges();
            }
        }

        public void SeedBooks()
        {
            //if (!_context.Books.Any())
            //{
            //    var bookData = System.IO.File.ReadAllText("BookSeedData.json");
            //    var books = JsonConvert.DeserializeObject<List<Book>>(bookData);

            //    foreach (var book in books)
            //    {
            //        book.AssetType = _context.AssetTypes.FirstOrDefault(a => a.Id == 1);
            //        book.Status = _context.Statuses.FirstOrDefault(a => a.Id == 1);
            //        book.CopiesAvailable = book.NumberOfCopies;
            //        _context.Add(book);
            //    }

            //    _context.SaveChanges();
            //}
        }

        public void SeedMedia()
        {
            //if (!_context.Medias.Any())
            //{
            //    var mediaData = System.IO.File.ReadAllText("MediaSeedData.json");
            //    var medias = JsonConvert.DeserializeObject<List<Media>>(mediaData);

            //    foreach (var media in medias)
            //    {
            //        media.CopiesAvailable = media.NumberOfCopies;
            //        media.AssetType = _context.AssetTypes.FirstOrDefault(a => a.Id == 2);
            //        media.Status = _context.Statuses.FirstOrDefault(a => a.Id == 1);
            //        _context.Add(media);
            //    }

            //    _context.SaveChanges();
            //}
        }
    }
}