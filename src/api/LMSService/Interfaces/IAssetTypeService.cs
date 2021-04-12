using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IAssetTypeService
    {
        Task<AssetType> AddAssetType(AssetType assetType);

        Task DeleteAssetType(AssetType assetType);

        Task<IEnumerable<AssetType>> GetAssetTypes();

        Task<AssetType> GetAssetType(int assetTypeId);
    }
}