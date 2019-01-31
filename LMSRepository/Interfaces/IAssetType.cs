using LMSLibrary.Models;
using System.Collections.Generic;

namespace LMSLibrary.DataAccess
{
    public interface IAssetType
    {

        List<AssetType> GetAll();
        AssetType Get(int id);
        void Add(AssetType newType);
    }
}
