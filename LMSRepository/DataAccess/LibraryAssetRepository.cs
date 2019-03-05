using LMSRepository.Interfaces.Models;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSRepository.Data;

namespace LMSRepository.Interfaces.DataAccess
{
    public class LibraryAssetRepository : ILibraryAssetRepository
    {
        private readonly DataContext _context;
        private readonly ILibraryRepository _libraryRepo;

        public LibraryAssetRepository(DataContext context, ILibraryRepository libraryRepo)
        {
            _context = context;
            _libraryRepo = libraryRepo;
        }

        public async Task<LibraryAsset> GetAsset(int id)
        {
            var asset = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
                .FirstOrDefaultAsync(a => a.Id == id);

            return asset;
        }

        public async Task<LibraryAsset> GetAssetByIsbn(string isbn)
        {
            var assets = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
                .FirstOrDefaultAsync(a => a.ISBN == isbn);

            return assets;
        }

        public async Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId)
        {
            var assets = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
                .Where(s => s.AuthorId == authorId).ToListAsync();

            return assets;
        }

        public async Task<IEnumerable<LibraryAsset>> GetAssetsByAuthorName(string name)
        {
            var assets = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
                .Where(s => s.Author.FullName == name).ToListAsync();

            return assets;
        }

        public async Task<IEnumerable<LibraryAsset>> SearchAssets(SearchAssetDto searchAsset)
        {
            var assets = from asset in _context.LibraryAssets
                         .Include(a => a.Author)
                         select asset;

            if (searchAsset != null)
            {
                if (!string.IsNullOrEmpty(searchAsset.Title))
                {
                    assets = assets.Where(a => a.Title.Contains(searchAsset.Title, StringComparison.OrdinalIgnoreCase));
                    return await assets.ToListAsync();
                }
                else if (!string.IsNullOrEmpty(searchAsset.AuthorName))
                {
                    assets = assets.Where(a => a.Author.FullName.Contains(searchAsset.AuthorName, StringComparison.OrdinalIgnoreCase));
                    return await assets.ToListAsync();
                }
            }

            return null;
        }

        public async Task<IEnumerable<LibraryAsset>> GetLibraryAssets()
        {
            var assets = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
                .OrderByDescending(o => o.Added)
                .ToListAsync();

            return assets;
        }

        public async Task<IEnumerable<LibraryAsset>> SearchLibraryAsset(string searchString)
        {
            var assets = from asset in _context.LibraryAssets
                        .Include(s => s.Author)
                        .Include(s => s.AssetType)
                         select asset;

            if (!string.IsNullOrEmpty(searchString))
            {
                assets = assets
                    .Where(s => s.Title.Contains(searchString)
                    || s.Author.LastName.Contains(searchString)
                    || s.Author.FirstName.Contains(searchString)
                    || s.ISBN.Contains(searchString));

                return await assets.ToListAsync();
            }

            return await GetLibraryAssets();
        }

        public void ReduceAssetCopiesAvailable(LibraryAsset libraryAsset)
        {
            libraryAsset.CopiesAvailable--;

            if (libraryAsset.CopiesAvailable == 0)
            {
                libraryAsset.StatusId = (int)EnumStatus.Unavailable;
            }
        }
    }
}