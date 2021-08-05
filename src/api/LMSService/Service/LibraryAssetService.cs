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
    public class LibraryAssetService : BaseService<LibraryAsset, LibraryAssetForDetailedDto, LibraryAssetForListDto, LibraryAssetService>, ILibraryAssetService
    {
        public LibraryAssetService(DataContext context, ILogger<LibraryAssetService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(context, mapper, logger, httpContextAccessor)
        {
        }

        public async Task<LibraryAssetForDetailedDto> AddAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            LibraryAsset asset = Mapper.Map<LibraryAsset>(libraryAssetForCreation);

            asset.SetCopiesAvailable();

            Context.Add(asset);
            await Context.SaveChangesAsync();

            Logger.LogInformation($"added {asset.Title} with ID: {asset.Id}");

            return Mapper.Map<LibraryAssetForDetailedDto>(asset);
        }

        public async Task AddAsset(List<LibraryAssetForCreationDto> libraryAssetForCreations)
        {
            List<LibraryAsset> assets = Mapper.Map<List<LibraryAsset>>(libraryAssetForCreations);

            foreach (LibraryAsset asset in assets)
            {

                asset.SetCopiesAvailable();
            }


            Context.AddRange(assets);
            await Context.SaveChangesAsync();
        }

        public async Task<LmsResponseHandler<LibraryAssetForDetailedDto>> DeleteAsset(int assetId)
        {
            LibraryAsset asset = await GetAsset(assetId);

            if (asset != null)
            {
                Context.Remove(asset);
                await Context.SaveChangesAsync();
                // TODO log who performed the action

                Logger.LogInformation($"{asset.Id} was deleted by user {GetLoggedInUserId()}");
                return LmsResponseHandler<LibraryAssetForDetailedDto>.Successful();
            }

            return LmsResponseHandler<LibraryAssetForDetailedDto>.Failed($"Selected item does not exist");
        }

        public async Task<LmsResponseHandler<LibraryAssetForDetailedDto>> EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            LibraryAsset asset = await GetAsset(libraryAssetForUpdate.Id);

            if (asset != null)
            {
                Mapper.Map(libraryAssetForUpdate, asset);

                asset.SetToAvailable();

                Context.Update(asset);


                await Context.SaveChangesAsync();
                Logger.LogInformation($"assetId: {libraryAssetForUpdate.Id} was edited");

                return LmsResponseHandler<LibraryAssetForDetailedDto>.Successful(Mapper.Map<LibraryAssetForDetailedDto>(await GetAsset(asset.Id)));
            }

            return LmsResponseHandler<LibraryAssetForDetailedDto>.Failed($"Item with title: '{libraryAssetForUpdate.Title}' does not exist");
        }

        public async Task<PagedList<LibraryAssetForListDto>> GetPaginatedAssets(PaginationParams paginationParams)
        {
            IQueryable<LibraryAsset> assets = Context.LibraryAssets.AsNoTracking()
                .Include(p => p.Photo)
                .Include(p => p.AssetCategories)
                .ThenInclude(c => c.Category)
                .Include(s => s.AssetAuthors.OrderBy(o => o.Order))
                .ThenInclude(a => a.Author)
                .AsQueryable();

            return await FilterAssets(paginationParams, assets);
        }

        public async Task<PagedList<LibraryAssetForListDto>> GetAssetsByAuthor(PaginationParams paginationParams, int authorId)
        {
            IQueryable<LibraryAsset> assets = Context.LibraryAssets.AsNoTracking()
                .Include(c => c.AssetCategories)
                    .ThenInclude(t => t.Category)
                    .Where(x => x.AssetAuthors.Any(t => t.AuthorId == authorId))
                    .AsQueryable();

            return await FilterAssets(paginationParams, assets);
        }

        public async Task<LmsResponseHandler<LibraryAssetForDetailedDto>> GetAssetWithDetails(int assetId)
        {
            return MapDetailReturn(await GetAsset(assetId));
        }

        private async Task<LibraryAsset> GetAsset(int assetId)
        {
            return await Context.LibraryAssets
                .Include(p => p.Photo)
                .Include(p => p.AssetCategories)
                .ThenInclude(ba => ba.Category)
                .Include(s => s.AssetAuthors.OrderBy(o => o.Order))
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

            return await MapPagination(assets, paginationParams);
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
