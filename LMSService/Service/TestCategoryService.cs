using LMSRepository.Data;
using LMSRepository.Models;
using LMSService.Interfacees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Service
{
    public class TestCategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public TestCategoryService(DataContext context)
        {
            _context = context;
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

        public Task<IEnumerable<Category>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetCategory(int categoryId)
        {
            var category = _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId);

            return category;
        }
    }
}
