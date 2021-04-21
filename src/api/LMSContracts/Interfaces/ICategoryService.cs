using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> AddCategory(Category category);

        Task DeleteCategory(Category category);

        Task<IEnumerable<Category>> GetCategories();

        Task<Category> GetCategory(int categoryId);
    }
}
