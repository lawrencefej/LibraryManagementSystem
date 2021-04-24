using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSContracts.Repository;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSRepository.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await FindAll().OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await GetByID(id);
        }

        public Category AddCategory(Category category)
        {
            return Create(category);
        }

        public Category UpdateCategory(Category category)
        {
            return Update(category);
        }

        public void DeleteCategory(Category category)
        {
            Delete(category);
        }
    }
}
