using LMSRepository.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface ILibraryAssestService
    {
        Task<LibraryAssetForDetailedDto> AddAsset(LibraryAssetForCreationDto libraryAssetForCreation);

        Task DeleteAsset(int assetId);

        Task EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate);

        Task<IEnumerable<LibraryAssetForListDto>> GetAllAssets();

        Task<LibraryAssetForDetailedDto> GetAsset(int assetId);

        Task<LibraryAssetForDetailedDto> GetAssetByIsbn(string isbn);

        Task<IEnumerable<LibraryAssetForListDto>> GetAssetsByAuthor(int authorId);

        Task<IEnumerable<LibraryAssetForListDto>> SearchLibraryAsset(string searchString);

        Task<IEnumerable<LibraryAssetForListDto>> SearchLibraryAsset(SearchAssetDto searchAsset);
    }
}