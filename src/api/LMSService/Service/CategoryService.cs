using System.Collections.Generic;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;

namespace LMSService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<Category> AddCategory(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteCategory(Category category)
        {
            _context.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await _context.Category.ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            var category = await _context.Category.FirstOrDefaultAsync(x => x.Id == categoryId);

            return category;
        }
    }
}
