using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfacees
{
    public interface ILibraryAssetService
    {
        Task<LibraryAsset> AddAsset(LibraryAsset asset);

        Task DeleteAsset(int assetId);
        Task EditAsset(LibraryAsset libraryAssetForUpdate);
        Task<LibraryAsset> GetAsset(int assetId);
        Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId);
        Task<IEnumerable<LibraryAsset>> SearchLibraryAsset(string searchString);

        Task<PagedList<LibraryAsset>> GetAllAsync(PaginationParams paginationParams);
    }
}