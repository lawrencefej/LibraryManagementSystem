using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface ILibraryAssetService
    {
        Task<LibraryAsset> AddAsset(LibraryAsset asset);

        Task DeleteAsset(LibraryAsset asset);

        Task EditAsset(LibraryAsset libraryAssetForUpdate);

        Task<LibraryAsset> GetAsset(int assetId);

        Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId);

        Task<IEnumerable<LibraryAsset>> SearchAvalableLibraryAsset(string searchString);

        Task<PagedList<LibraryAsset>> GetAllAsync(PaginationParams paginationParams);
    }
}
