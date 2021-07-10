using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class LibraryAssetService : BaseService<LibraryAssetForDetailedDto>, ILibraryAssetService
    {
        private readonly ILogger<LibraryAssetService> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public LibraryAssetService(DataContext context, ILogger<LibraryAssetService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        public async Task<LibraryAssetForDetailedDto> AddAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            LibraryAsset asset = _mapper.Map<LibraryAsset>(libraryAssetForCreation);

            asset.SetCopiesAvailable();

            _context.Add(asset);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"added {asset.Title} with ID: {asset.Id}");

            return _mapper.Map<LibraryAssetForDetailedDto>(asset);
        }

        public async Task AddAsset(List<LibraryAssetForCreationDto> libraryAssetForCreations)
        {
            List<LibraryAsset> assets = _mapper.Map<List<LibraryAsset>>(libraryAssetForCreations);

            foreach (LibraryAsset asset in assets)
            {

                asset.SetCopiesAvailable();
            }


            _context.AddRange(assets);
            await _context.SaveChangesAsync();
        }

        public async Task<LmsResponseHandler<LibraryAssetForDetailedDto>> DeleteAsset(int assetId)
        {
            LibraryAsset asset = await GetAsset(assetId);

            if (asset != null)
            {
                _context.Remove(asset);
                await _context.SaveChangesAsync();
                // TODO log who performed the action

                _logger.LogInformation($"{asset.Id} was deleted by user {GetLoggedInUserId()}");
                return LmsResponseHandler<LibraryAssetForDetailedDto>.Successful();
            }

            return LmsResponseHandler<LibraryAssetForDetailedDto>.Failed($"Selected item does not exist");
        }

        public async Task<LmsResponseHandler<LibraryAssetForDetailedDto>> EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            LibraryAsset asset = await GetAsset(libraryAssetForUpdate.Id);

            if (asset != null)
            {
                _mapper.Map(libraryAssetForUpdate, asset);

                asset.SetToAvailable();

                _context.Update(asset);


                await _context.SaveChangesAsync();
                _logger.LogInformation($"assetId: {libraryAssetForUpdate.Id} was edited");

                return LmsResponseHandler<LibraryAssetForDetailedDto>.Successful(_mapper.Map<LibraryAssetForDetailedDto>(asset));
            }

            return LmsResponseHandler<LibraryAssetForDetailedDto>.Failed($"Item with title: '{libraryAssetForUpdate.Title}' does not exist");
        }

        public async Task<PagedList<LibraryAssetForListDto>> GetPaginatedAssets(PaginationParams paginationParams)
        {
            IQueryable<LibraryAsset> assets = _context.LibraryAssets.AsNoTracking()
                .Include(p => p.Photo)
                .Include(p => p.AssetCategories)
                    .ThenInclude(c => c.Category)
                .Include(s => s.AssetAuthors)
                    .ThenInclude(a => a.Author)
                .AsQueryable();

            return await FilterAssets(paginationParams, assets);
        }

        public async Task<PagedList<LibraryAssetForListDto>> GetAssetsByAuthor(PaginationParams paginationParams, int authorId)
        {
            IQueryable<LibraryAsset> assets = _context.LibraryAssets.AsNoTracking()
                .Include(c => c.AssetCategories)
                    .ThenInclude(t => t.Category)
                    .Where(x => x.AssetAuthors.Any(t => t.AuthorId == authorId))
                    .AsQueryable();

            return await FilterAssets(paginationParams, assets);
        }

        public async Task<LmsResponseHandler<LibraryAssetForDetailedDto>> GetAssetWithDetails(int assetId)
        {
            return MapLibraryAsset(await GetAsset(assetId));
        }

        private async Task<LibraryAsset> GetAsset(int assetId)
        {
            return await _context.LibraryAssets
                .Include(p => p.Photo)
                .Include(p => p.AssetCategories)
                    .ThenInclude(ba => ba.Category)
                .Include(s => s.AssetAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(a => a.Id == assetId);
        }

        private async Task<PagedList<LibraryAssetForListDto>> FilterAssets(PaginationParams paginationParams, IQueryable<LibraryAsset> assets)
        {
            if (!string.IsNullOrWhiteSpace(paginationParams.SearchString))
            {
                assets = assets.Where(x => x.Title.Contains(paginationParams.SearchString));
            }

            assets = paginationParams.SortDirection == "desc" ? assets.OrderByDescending(x => x.Title) : assets.OrderBy(x => x.Title);

            PagedList<LibraryAsset> assetsList = await PagedList<LibraryAsset>.CreateAsync(assets, paginationParams.PageNumber, paginationParams.PageSize);

            PagedList<LibraryAssetForListDto> assetToReturn = _mapper.Map<PagedList<LibraryAssetForListDto>>(assetsList);

            return PagedListMapper<LibraryAssetForListDto, LibraryAsset>.MapPagedList(assetToReturn, assetsList);
        }

        private LmsResponseHandler<LibraryAssetForDetailedDto> MapLibraryAsset(LibraryAsset asset)
        {
            if (asset != null)
            {
                LibraryAssetForDetailedDto assetForReturn = _mapper.Map<LibraryAssetForDetailedDto>(asset);

                return LmsResponseHandler<LibraryAssetForDetailedDto>.Successful(assetForReturn);
            }

            return LmsResponseHandler<LibraryAssetForDetailedDto>.Failed("");
        }

        // private bool DoesIsbnExist(string isbn)
        // {
        //     return !_context.LibraryAssets.Any(p => p.ISBN == isbn);
        // }

        // private bool DoesCategoryExist(int id)
        // {
        //     return _context.Categories.Any(a => a.Id == id);
        // }

        // private bool DoesAuthorExist(int id)
        // {
        //     return _context.Authors.Any(a => a.Id == id);
        // }
    }
}
