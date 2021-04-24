using System.Collections.Generic;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSContracts.Repository;
using LMSEntities.Models;

namespace LMSService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Category> AddCategory(Category category)
        {
            unitOfWork.Category.AddCategory(category);
            await unitOfWork.SaveChangesAsync();

            return category;
        }

        public async Task DeleteCategory(Category category)
        {
            unitOfWork.Category.DeleteCategory(category);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await unitOfWork.Category.GetCategories();
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            return await unitOfWork.Category.GetCategoryById(categoryId);
        }
    }
}
