using System.Collections.Generic;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSContracts.Repository;
using LMSEntities.Models;

namespace LMSService.Service
{
    public class AssetTypeService : IAssetTypeService
    {
        private readonly IUnitOfWork unitOfWork;

        public AssetTypeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<AssetType> AddAssetType(AssetType assetType)
        {
            _ = unitOfWork.AssetType.AddAssetType(assetType);
            await unitOfWork.SaveChangesAsync();
            return assetType;
        }

        public async Task<AssetType> GetAssetType(int assetTypeId)
        {
            return await unitOfWork.AssetType.GetAssetType(assetTypeId);
        }

        public async Task<IEnumerable<AssetType>> GetAssetTypes()
        {
            return await unitOfWork.AssetType.GetAssetTypes();
        }

        public async Task<AssetType> UpdateAssetType(AssetType assetType)
        {
            _ = unitOfWork.AssetType.UpdateAssetType(assetType);
            await unitOfWork.SaveChangesAsync();
            return assetType;
        }

        public async Task DeleteAssetType(AssetType assetType)
        {
            unitOfWork.AssetType.DeleteAssetType(assetType);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
