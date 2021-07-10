using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;

namespace LMSContracts.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> AddCategory(CategoryDto categoryForCreation);

        Task<LmsResponseHandler<CategoryDto>> DeleteCategory(int categoryId);

        Task<IEnumerable<Category>> GetCategories();

        Task<Category> GetCategory(int categoryId);
    }
}
