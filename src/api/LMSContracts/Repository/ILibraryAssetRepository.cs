using System.Threading.Tasks;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Repository
{
    public interface ILibraryAssetRepository : IRepositoryBase<LibraryAsset>
    {
        Task<PagedList<LibraryAsset>> GetPaginatedAssetsAsync(PaginationParams paginationParams);
        Task<LibraryAsset> GetAssetByIdAsync(int assetID);
        void CreateAsset(LibraryAsset asset);
        void UpdateAsset(LibraryAsset libraryAssetForUpdate);
        void DeleteAsset(LibraryAsset libraryAsset);
    }
}
