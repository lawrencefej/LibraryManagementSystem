using System.Threading.Tasks;
using LMSContracts.Repository;
using LMSRepository.Data;

namespace LMSRepository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dataContext;
        private IAssetTypeRepository assetType;
        private ICategoryRepository category;
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

        public ICategoryRepository Category
        {
            get
            {
                if (category == null)
                {
                    category = new CategoryRepository(dataContext);
                }

                return category;
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
