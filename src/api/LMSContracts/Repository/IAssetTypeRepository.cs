using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.Models;

namespace LMSContracts.Repository
{
    public interface IAssetTypeRepository : IRepositoryBase<AssetType>
    {
        Task<IEnumerable<AssetType>> GetAssetTypes();
        Task<AssetType> GetAssetType(int assetTypeId);
        AssetType AddAssetType(AssetType assetType);
        AssetType UpdateAssetType(AssetType assetType);
        void DeleteAssetType(AssetType assetType);
    }
}
