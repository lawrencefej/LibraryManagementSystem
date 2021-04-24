using System.Threading.Tasks;

namespace LMSContracts.Repository
{
    public interface IUnitOfWork
    {
        // ILibraryAssetRepository LibraryAsset { get; }
        IAssetTypeRepository AssetType { get; }
        Task SaveChangesAsync();
    }
}
