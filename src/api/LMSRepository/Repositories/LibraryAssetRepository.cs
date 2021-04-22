using System.Threading.Tasks;
using LMSContracts.Repository;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Repositories
{
    public class LibraryAssetRepository : RepositoryBase<LibraryAsset>, ILibraryAssetRepository
    {
        public LibraryAssetRepository(DataContext dataContext) : base(dataContext)
        {

        }

        public async Task<LibraryAsset> GetAssetByIdAsync(int assetId)
        {
            return await FindByCondition(asset => asset.Id.Equals(assetId)).FirstOrDefaultAsync();
        }

        public Task<PagedList<LibraryAsset>> GetPaginatedAssetsAsync(PaginationParams paginationParams)
        {
            throw new System.NotImplementedException();
        }

        public void CreateAsset(LibraryAsset libraryAsset)
        {
            Create(libraryAsset);
        }
        public void UpdateAsset(LibraryAsset libraryAsset)
        {
            Update(libraryAsset);
        }

        public void DeleteAsset(LibraryAsset libraryAsset)
        {
            Delete(libraryAsset);
        }
    }
}
