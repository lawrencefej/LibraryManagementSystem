﻿using LMSLibrary.Models;
using LMSRepository.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSLibrary.DataAccess
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

        public async Task<IEnumerable<LibraryAsset>> GetLibraryAssets()
        {
            var assets = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
                .ToListAsync();

            return assets;
        }

        public async Task<IEnumerable<LibraryAsset>> SearchLibraryAsset(string searchString)
        {
            var assets = from asset in _context.LibraryAssets
                         select asset;

            if (!string.IsNullOrEmpty(searchString))
            {
                assets = assets.Where(s => s.Title.Contains(searchString));
            }

            return await assets.ToListAsync();
        }

        public void ReduceAssetCopiesAvailable(LibraryAsset libraryAsset)
        {
            libraryAsset.CopiesAvailable--;

            if (libraryAsset.CopiesAvailable == 0)
            {
                libraryAsset.StatusId = (int)StatusEnum.Unavailable;
            }
        }
    }
}