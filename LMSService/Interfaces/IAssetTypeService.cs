using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IAssetTypeService
    {
        Task<AssetType> AddAssetType(AssetType assetType);

        Task DeleteAssetType(int assetTypeId);

        Task EditAuthor(AssetType assetType);

        Task<IEnumerable<AssetType>> GetAssetTypes();

        Task<AssetType> GetAssetType(int assetTypeId);
    }
}