using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.Models;

namespace LMSContracts.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<Category> GetCategoryById(int id);

        Category AddCategory(Category category);

        Category UpdateCategory(Category category);

        void DeleteCategory(Category category);

    }
}
