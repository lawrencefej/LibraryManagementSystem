using LMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LMSLibrary.Data
{
    public interface ILibraryAssetRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        Task<bool> SaveAll();
        void AddAsset(LibraryAsset newLibraryAsset);
        Task<List<LibraryAsset>> GetLibraryAssets();
        Task<LibraryAsset> GetAsset(int id);
        Task<List<LibraryAsset>> GetByAuthorOrDirector(string author);
        void AddAssetType(LibraryAssetType libraryAssetType);
        IReadOnlyList<string> ValidateCheckout(LibraryAsset libraryAsset, LibraryCard libraryCard);
        LibraryAsset ReduceAssetCopiesAvailable(LibraryAsset libraryAsset);

    }
}
