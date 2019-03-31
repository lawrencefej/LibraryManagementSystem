using LMSRepository.Interfaces;
using LMSRepository.Interfaces.Models;
using LMSService.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<Category> AddCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task EditCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var assetTypesToReturn = await _categoryRepository.GetAll();

            return assetTypesToReturn;
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            var assetTypeToReturn = await _categoryRepository.GetCategory(categoryId);

            return assetTypeToReturn;
        }
    }
}