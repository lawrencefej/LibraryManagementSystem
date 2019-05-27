using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSService.Dto
{
    public interface ILibraryAssestService
    {
        Task<LibraryAssetForDetailedDto> AddAsset(LibraryAssetForCreationDto libraryAssetForCreation);

        Task DeleteAsset(int assetId);

        Task EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate);

        Task<IEnumerable<LibraryAssetForListDto>> GetAllAssets();

        IQueryable<LibraryAssetForListDto> GetAll();

        Task<PagedList<LibraryAssetForListDto>> GetAllAssets(PaginationParams paginationParams);

        Task<LibraryAssetForDetailedDto> GetAsset(int assetId);

        Task<LibraryAssetForDetailedDto> GetAssetByIsbn(string isbn);

        Task<IEnumerable<LibraryAssetForListDto>> GetAssetsByAuthor(int authorId);

        Task<IEnumerable<LibraryAssetForListDto>> SearchLibraryAsset(string searchString);

        Task<IEnumerable<LibraryAssetForListDto>> SearchLibraryAsset(SearchAssetDto searchAsset);

        Task<PagedList<LibraryAsset>> GetAllAsync(PaginationParams paginationParams);
    }
}