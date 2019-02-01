using LMSRepository.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface ILibraryAssestService
    {
        Task AddAsset(LibraryAssetForCreationDto libraryAssetForCreation);

        Task DeleteAsset(int assetId);

        Task EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate);

        Task<IEnumerable<LibraryAssetForDetailedDto>> GetAllAssets();

        Task<LibraryAssetForDetailedDto> GetAsset(int assetId);

        Task<LibraryAssetForDetailedDto> GetAssetByIsbn(string isbn);

        Task<IEnumerable<LibraryAssetForDetailedDto>> GetAssetsByAuthor(int authorId);

        Task<IEnumerable<LibraryAssetForDetailedDto>> SearchLibraryAsset(string searchString);
    }
}