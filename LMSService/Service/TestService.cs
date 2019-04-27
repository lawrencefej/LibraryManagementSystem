using LMSRepository.Data;
using LMSRepository.DataAccess;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class TestService : BaseRepository<LibraryAsset>, ITestService
    {
        public TestService(DataContext context) : base(context)
        {
        }

        public Task<LibraryAssetForDetailedDto> AddAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsset(int assetId)
        {
            throw new NotImplementedException();
        }

        public Task EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LibraryAssetForListDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibraryAssetForListDto>> GetAllAssets()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<LibraryAsset>> GetAllAssetsAsync(PaginationParams paginationParams)
        {
            var assets = FindAll()
                .Include(p => p.Photo)
                .Include(a => a.AssetType)
                .Include(s => s.Status)
                .Include(s => s.Author)
                .OrderByDescending(o => o.Title);

            if (!string.IsNullOrEmpty(paginationParams.OrderBy))
            {
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

            return await PagedList<LibraryAsset>.CreateAsync(assets, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public Task<LibraryAssetForDetailedDto> GetAsset(int assetId)
        {
            throw new NotImplementedException();
        }

        public Task<LibraryAssetForDetailedDto> GetAssetByIsbn(string isbn)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibraryAssetForListDto>> GetAssetsByAuthor(int authorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibraryAssetForListDto>> SearchLibraryAsset(string searchString)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibraryAssetForListDto>> SearchLibraryAsset(SearchAssetDto searchAsset)
        {
            throw new NotImplementedException();
        }
    }
}