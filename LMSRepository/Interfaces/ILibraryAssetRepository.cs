using LMSLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSLibrary.DataAccess
{
    public interface ILibraryAssetRepository
    {
        Task<LibraryAsset> GetAsset(int id);

        Task<LibraryAsset> GetAssetByIsbn(string isbn);

        Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId);

        Task<IEnumerable<LibraryAsset>> GetLibraryAssets();

        void ReduceAssetCopiesAvailable(LibraryAsset libraryAsset);

        Task<IEnumerable<LibraryAsset>> SearchLibraryAsset(string searchString);
    }
}