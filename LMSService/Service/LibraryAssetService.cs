using LMSRepository.Data;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Exceptions;
using LMSService.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class LibraryAssetService : ILibraryAssetService
    {
        private readonly ILogger<LibraryAssetService> _logger;
        private readonly DataContext _context;

        public LibraryAssetService(DataContext context, ILogger<LibraryAssetService> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<LibraryAsset> AddAsset(LibraryAsset asset)
        {
            asset.StatusId = (int)EnumStatus.Available;
            asset.CopiesAvailable = asset.NumberOfCopies;

            await _context.AddAsync(asset);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"added {asset.Title} with ID: {asset.Id}");

            return asset;
        }

        public async Task DeleteAsset(int assetId)
        {
            var asset = await _context.LibraryAssets
                .FirstOrDefaultAsync(a => a.Id == assetId).ConfigureAwait(false);

            if (asset == null)
            {
                _logger.LogWarning($"assetId: {assetId} was not found");
                throw new NoValuesFoundException($"assetId: {assetId} was not found");
            }

            _context.Remove(asset);
            await _context.SaveChangesAsync();
            // TODO log who performed the action

            _logger.LogInformation($"assetId: {asset.Id} was deleted");
            return;
        }

        public async Task EditAsset(LibraryAsset libraryAssetForUpdate)
        {
            // TODO potentially make central
            if (libraryAssetForUpdate.CopiesAvailable > 0)
            {
                libraryAssetForUpdate.StatusId = (int)EnumStatus.Available;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"assetId: {libraryAssetForUpdate.Id} was edited");
            return;
        }

        public async Task<PagedList<LibraryAsset>> GetAllAsync(PaginationParams paginationParams)
        {
            var assets = _context.LibraryAssets
                .Include(a => a.AssetType)
                .Include(s => s.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(paginationParams.OrderBy))
            {
                // TODO make this cleaner
                switch (paginationParams.OrderBy)
                {
                    case "created":
                        assets = assets.OrderByDescending(u => u.Title);
                        break;

                    default:
                        assets = assets.OrderByDescending(u => u.Author);
                        break;
                }
            }
            else
            {
                assets = assets.OrderBy(x => x.Title);
            }

            return await PagedList<LibraryAsset>.CreateAsync(assets, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<LibraryAsset> GetAsset(int assetId)
        {
            var asset = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(p => p.Category)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
                .FirstOrDefaultAsync(x => x.Id == assetId);

            return asset;
        }

        public async Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId)
        {
            var assets = await _context.LibraryAssets
                .Include(a => a.AssetType)
                .Include(s => s.Author)
                .Where(x => x.AuthorId == authorId)
                .ToListAsync();

            return assets;
        }

        public async Task<IEnumerable<LibraryAsset>> SearchLibraryAsset(string searchString)
        {
            // TODO make sure it is case insensitive
            var assets = _context.LibraryAssets
                        .Include(s => s.Author)
                        .Include(s => s.AssetType)
                        .AsQueryable();

            assets = assets
                .Where(s => s.Title.Contains(searchString)
                || s.Author.LastName.Contains(searchString)
                || s.Author.FirstName.Contains(searchString)
                || s.ISBN.Contains(searchString));
            await assets.ToListAsync();

            return assets;
        }
    }
}