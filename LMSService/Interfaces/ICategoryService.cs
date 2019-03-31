using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Dto
{
    public interface ICategoryService
    {
        Task<Category> AddCategory(Category category);

        Task DeleteCategory(int categoryId);

        Task EditCategory(Category category);

        Task<IEnumerable<Category>> GetCategories();

        Task<Category> GetCategory(int categoryId);
    }
}