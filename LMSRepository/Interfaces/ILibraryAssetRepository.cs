using LMSLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSLibrary.Data
{
    public interface ILibraryAssetRepository
    {
        Task<IEnumerable<LibraryAsset>> GetLibraryAssets();
        Task<LibraryAsset> GetAsset(int id);
        //Task<List<LibraryAsset>> GetByAuthorOrDirector(string author);

    }
}
