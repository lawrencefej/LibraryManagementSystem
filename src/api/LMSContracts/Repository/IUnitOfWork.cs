using System.Threading.Tasks;

namespace LMSContracts.Repository
{
    public interface IUnitOfWork
    {
        // ILibraryAssetRepository LibraryAsset { get; }
        IAssetTypeRepository AssetType { get; }
        ICategoryRepository Category { get; }
        Task SaveChangesAsync();
    }
}
