using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.Enumerations;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using LMSService.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            // asset.StatusId = (int)StatusEnum.Available;
            asset.Status = LibraryAssetStatus.Available;
            asset.CopiesAvailable = asset.NumberOfCopies;

            // asset.AssetType = null;
            // asset.Author = null;
            asset.Category = null;

            _context.Add(asset);
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
            if (libraryAssetForUpdate.CopiesAvailable > 0)
            {
                // libraryAssetForUpdate.StatusId = (int)StatusEnum.Available;
                libraryAssetForUpdate.Status = LibraryAssetStatus.Available;
            }

            _context.Update(libraryAssetForUpdate);

            await _context.SaveChangesAsync();
            _logger.LogInformation($"assetId: {libraryAssetForUpdate.Id} was edited");
            return;
        }

        public async Task<PagedList<LibraryAsset>> GetAllAsync(PaginationParams paginationParams)
        {
            var assets = _context.LibraryAssets.AsNoTracking()
                .Include(p => p.Photo)
                .Include(p => p.Category)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.AssetAuthors)
                // .Include(s => s.Authors)
                .AsQueryable();

            if (!string.IsNullOrEmpty(paginationParams.SearchString))
            {
                assets = assets.Where(x => x.Title.Contains(paginationParams.SearchString));
            }

            if (paginationParams.SortDirection == "asc")
            {
                assets = assets.OrderBy(x => x.Title);
            }
            else if (paginationParams.SortDirection == "desc")
            {
                assets = assets.OrderByDescending(x => x.Title);
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
                .Include(s => s.AssetAuthors)
                // .Include(s => s.Authors)
                .FirstOrDefaultAsync(x => x.Id == assetId);

            return asset;
        }

        public async Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId)
        {
            var assets = await _context.LibraryAssets.AsNoTracking()
                .Include(a => a.AssetType)
                .Include(s => s.AssetAuthors)
                // .Include(s => s.Authors)
                // .Where(x => x.AuthorId == authorId)
                .ToListAsync();

            return assets;
        }

        public async Task<IEnumerable<LibraryAsset>> SearchAvalableLibraryAsset(string searchString)
        {
            // TODO make sure it is case insensitive
            var assets = _context.LibraryAssets.AsNoTracking()
                        .Include(p => p.Photo)
                        .Include(s => s.AssetAuthors)
                        // .Include(s => s.Authors)
                        .Include(s => s.AssetType)
                        .AsQueryable();

            // assets = assets.Where(x => x.StatusId == (int)StatusEnum.Available);
            assets = assets.Where(x => x.Status == LibraryAssetStatus.Available);

            assets = assets
                .Where(s => s.Title.Contains(searchString)
                // || s.Author.LastName.Contains(searchString)
                // || s.Author.FirstName.Contains(searchString)
                || s.ISBN.Contains(searchString));
            await assets.ToListAsync();

            return assets;
        }
    }
}
