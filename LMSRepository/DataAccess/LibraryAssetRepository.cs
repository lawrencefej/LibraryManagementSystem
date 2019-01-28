using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LMSLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSLibrary.Data
{
    public class LibraryAssetRepository : ILibraryAssetRepository
    {
        private readonly DataContext _context;

        public Checkout Checkout { get; }

        public LibraryAssetRepository(DataContext context)
        {
            _context = context;
        }

        public LibraryAssetRepository(Checkout checkout)
        {
            Checkout = checkout;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async void AddAsset(LibraryAsset newLibraryAsset)
        {
            await _context.AddAsync(newLibraryAsset);
        }

        public void AddAssetType(LibraryAssetType libraryAssetType)
        {
            throw new NotImplementedException();
        }

        public async Task<LibraryAsset> GetAsset(int id)
        {
            var asset = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(a => a.Id == id);

            return asset;
        }

        //public async Task<List<LibraryAsset>> GetByAuthorOrDirector(string author)
        //{
        //    var query = await GetLibraryAssets();

        //    var assets = query.Where(a => a.Author.FullName == author).ToList();

        //    return assets;
        //}

        public async Task<IEnumerable<LibraryAsset>> GetLibraryAssets()
        {
            var assets = await _context.LibraryAssets
                //.Include(p => p.Photo)
                //.Include(a => a.AssetType)
                //.Include(s => s.Status)
                .ToListAsync();

            return assets;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public  IReadOnlyList<string> ValidateCheckout(LibraryAsset libraryAsset, LibraryCard libraryCard)
        {
            var errors = new List<string>();

            if (libraryCard.Fees != 0)
            {
                //TODO remove comments when done testing
                //errors.Add("This user still has outstanding bills");
            }

            if (libraryAsset.Status.Name != "Available")
            {
                errors.Add($"There are no copies of {libraryAsset.Title} available at this time");
            }

            return errors;
        }

        public LibraryAsset ReduceAssetCopiesAvailable(LibraryAsset libraryAsset)
        {
            libraryAsset.CopiesAvailable--;

            if (libraryAsset.CopiesAvailable == 0)
            {
                libraryAsset.Status = _context.Statuses.FirstOrDefault(a => a.Id == 2);
            }

            Update(libraryAsset);

            return libraryAsset;
        }

        public ValidationResult Validate(LibraryAsset libraryAsset, LibraryCard libraryCard)
        {
            if (libraryCard.Fees != 0)
            {
                return new ValidationResult("This user still has outstanding bills");
                //TODO remove comments when done testing
                //errors.Add("This user still has outstanding bills");
            }

            if (libraryAsset.Status.Name != "Available")
            {
                return new ValidationResult($"There are no copies of {libraryAsset.Title} available at this time");
                //errors.Add($"There are no copies of {libraryAsset.Title} available at this time");
            }

            return new ValidationResult("");
        }
    }
}
