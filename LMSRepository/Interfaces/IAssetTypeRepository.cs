using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface IAssetTypeRepository
    {
        Task<IEnumerable<AssetType>> GetAll();

        Task<AssetType> Get(int assetTypeId);
    }
}