using LMSRepository.Interfaces;
using LMSRepository.Interfaces.Models;
using LMSService.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class AssetTypeService : IAssetTypeService
    {
        private readonly IAssetTypeRepository _assetTypeRepository;

        public AssetTypeService(IAssetTypeRepository assetTypeRepository)
        {
            _assetTypeRepository = assetTypeRepository;
        }

        public Task<AssetType> AddAssetType(AssetType assetType)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAssetType(int assetTypeId)
        {
            throw new NotImplementedException();
        }

        public Task EditAuthor(AssetType assetType)
        {
            throw new NotImplementedException();
        }

        public async Task<AssetType> GetAssetType(int assetTypeId)
        {
            var assetTypeToReturn = await _assetTypeRepository.Get(assetTypeId);

            return assetTypeToReturn;
        }

        public async Task<IEnumerable<AssetType>> GetAssetTypes()
        {
            var assetTypesToReturn = await _assetTypeRepository.GetAll();

            return assetTypesToReturn;
        }
    }
}