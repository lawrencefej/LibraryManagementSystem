using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;

namespace LMSContracts.Interfaces
{
    public interface ILibraryAssetService
    {
        Task<LibraryAssetForDetailedDto> AddAsset(LibraryAssetForCreationDto libraryAssetForCreation);

        Task<LmsResponseHandler<LibraryAssetForDetailedDto>> DeleteAsset(LibraryAssetForDetailedDto assetForDel);

        Task<LmsResponseHandler<LibraryAssetForDetailedDto>> EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate);

        Task<LmsResponseHandler<LibraryAssetForDetailedDto>> GetAssetWithDetails(int assetId);

        Task<PagedList<LibraryAssetForListDto>> GetAssetsByAuthor(PaginationParams paginationParams, int authorId);

        Task<PagedList<LibraryAssetForListDto>> GetPaginatedAssets(PaginationParams paginationParams);
        Task AddAsset(List<LibraryAssetForCreationDto> libraryAssetForCreations);
    }
}
