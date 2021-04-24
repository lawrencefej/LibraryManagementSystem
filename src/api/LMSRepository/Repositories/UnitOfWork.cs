using System.Threading.Tasks;
using LMSContracts.Repository;
using LMSRepository.Data;

namespace LMSRepository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dataContext;
        private IAssetTypeRepository assetType;
        public IAssetTypeRepository AssetType
        {
            get
            {
                if (assetType == null)
                {
                    assetType = new AssetTypeRepository(dataContext);
                }

                return assetType;
            }
        }
        public UnitOfWork(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task SaveChangesAsync()
        {
            await dataContext.SaveChangesAsync();
        }
    }
}
