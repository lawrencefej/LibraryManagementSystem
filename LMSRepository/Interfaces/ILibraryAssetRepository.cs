using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface ILibraryAssetRepository
    {
        Task<LibraryAsset> GetAsset(int id);

        Task<LibraryAsset> GetAssetByIsbn(string isbn);

        Task<IEnumerable<LibraryAsset>> GetAssetsByAuthor(int authorId);

        Task<IEnumerable<LibraryAsset>> GetLibraryAssets();

        Task<PagedList<LibraryAsset>> GetPagedLibraryAssetsAsync(PaginationParams paginationParams);

        IQueryable<LibraryAsset> GetAll();

        void ReduceAssetCopiesAvailable(LibraryAsset libraryAsset);

        Task<IEnumerable<LibraryAsset>> SearchLibraryAsset(string searchString);

        Task<IEnumerable<LibraryAsset>> SearchAssets(SearchAssetDto searchAsset);
    }
}