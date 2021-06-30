using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class LibraryAssetService : ILibraryAssetService
    {
        private readonly ILogger<LibraryAssetService> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public LibraryAssetService(DataContext context, ILogger<LibraryAssetService> logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<LibraryAsset> AddAsset(LibraryAsset asset)
        {
            asset.Status = LibraryAssetStatus.Available;
            asset.CopiesAvailable = asset.NumberOfCopies;

            _context.Add(asset);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"added {asset.Title} with ID: {asset.Id}");

            return asset;
        }

        public async Task<LibraryAssetForDetailedDto> AddAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            LibraryAsset asset = _mapper.Map<LibraryAsset>(libraryAssetForCreation);

            asset.CopiesAvailable = asset.NumberOfCopies;

            _context.Add(asset);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"added {asset.Title} with ID: {asset.Id}");

            return _mapper.Map<LibraryAssetForDetailedDto>(asset);
        }

        public async Task DeleteAsset(LibraryAsset asset)
        {

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
                libraryAssetForUpdate.Status = LibraryAssetStatus.Available;
            }

            _context.Update(libraryAssetForUpdate);

            await _context.SaveChangesAsync();
            _logger.LogInformation($"assetId: {libraryAssetForUpdate.Id} was edited");
            return;
        }

        public async Task<PagedList<LibraryAsset>> GetAllAsync(PaginationParams paginationParams)
        {
            IQueryable<LibraryAsset> assets = _context.LibraryAssets.AsNoTracking()
                .Include(p => p.Photo)
                .Include(p => p.AssetCategories)
                    .ThenInclude(c => c.Category)
                .Include(s => s.AssetAuthors)
                    .ThenInclude(a => a.Author)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(paginationParams.SearchString))
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
            LibraryAsset asset = await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(p => p.AssetCategories)
                    .ThenInclude(ba => ba.Category)
                .Include(s => s.AssetAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(x => x.Id == assetId);

            return asset;
        }

        public async Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId)
        {
            List<LibraryAsset> assets = await _context.LibraryAssets.AsNoTracking()
                .Include(c => c.AssetCategories)
                    .ThenInclude(t => t.Category)
                .Where(x => x.AssetAuthors.Any(t => t.AuthorId == authorId))
                .ToListAsync();

            return assets;
        }

        public async Task<IEnumerable<LibraryAsset>> SearchAvalableLibraryAsset(string searchString)
        {
            // TODO make sure it is case insensitive
            IQueryable<LibraryAsset> assets = _context.LibraryAssets.AsNoTracking()
                        .Include(p => p.Photo)
                        .Include(s => s.AssetAuthors)
                            .ThenInclude(a => a.Author)
                        .AsQueryable();

            assets = assets.Where(x => x.Status == LibraryAssetStatus.Available);

            assets = assets
                .Where(s => s.Title.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.AssetAuthors.Any(a => a.Author.FullName.Contains(searchString))
                || s.ISBN.Contains(searchString));
            await assets.ToListAsync();

            return assets;
        }
    }
}
