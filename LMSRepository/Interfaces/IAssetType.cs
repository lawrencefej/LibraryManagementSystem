using LMSRepository.Interfaces.Models;
using System.Collections.Generic;

namespace LMSRepository.Interfaces
{
    public interface IAssetType
    {
        List<AssetType> GetAll();

        AssetType Get(int id);

        void Add(AssetType newType);
    }
}