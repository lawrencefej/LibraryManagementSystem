using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Repository;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Repositories
{
    public class AssetTypeRepository : RepositoryBase<AssetType>, IAssetTypeRepository
    {
        public AssetTypeRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<IEnumerable<AssetType>> GetAssetTypes()
        {
            return await FindAll().OrderBy(asset => asset.Name).ToListAsync();
        }

        public async Task<AssetType> GetAssetType(int assetTypeId)
        {
            return await GetByID(assetTypeId);
        }
        public AssetType AddAssetType(AssetType assetType)
        {
            return Create(assetType);
        }

        public AssetType UpdateAssetType(AssetType assetType)
        {
            return Update(assetType);
        }

        public void DeleteAssetType(AssetType assetType)
        {
            Delete(assetType);
        }
    }
}
