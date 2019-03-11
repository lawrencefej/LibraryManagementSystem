using LMSRepository.Data;
using LMSRepository.Interfaces;
using LMSRepository.Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.DataAccess
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            var categories = await _context.Category.ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            var category = await _context.Category.FirstOrDefaultAsync();

            return category;
        }
    }
}