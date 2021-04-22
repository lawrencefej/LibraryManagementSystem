using System.Threading.Tasks;

namespace LMSContracts.Repository
{
    public interface IUnitOfWork
    {
        ILibraryAssetRepository LibraryAsset { get; }
        Task SaveChangesAsync();
    }
}
