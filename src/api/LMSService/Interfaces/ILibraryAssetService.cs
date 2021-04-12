using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface ILibraryAssetService
    {
        Task<LibraryAsset> AddAsset(LibraryAsset asset);

        Task DeleteAsset(int assetId);

        Task EditAsset(LibraryAsset libraryAssetForUpdate);

        Task<LibraryAsset> GetAsset(int assetId);

        Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId);

        Task<IEnumerable<LibraryAsset>> SearchAvalableLibraryAsset(string searchString);

        Task<PagedList<LibraryAsset>> GetAllAsync(PaginationParams paginationParams);
    }
}