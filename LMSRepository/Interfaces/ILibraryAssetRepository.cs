using LMSRepository.Dto;
using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface ILibraryAssetRepository
    {
        Task<LibraryAsset> GetAsset(int id);

        Task<LibraryAsset> GetAssetByIsbn(string isbn);

        Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId);

        Task<IEnumerable<LibraryAsset>> GetLibraryAssets();

        void ReduceAssetCopiesAvailable(LibraryAsset libraryAsset);

        Task<IEnumerable<LibraryAsset>> SearchLibraryAsset(string searchString);

        Task<IEnumerable<LibraryAsset>> SearchAssets(SearchAssetDto searchAsset);
    }
}