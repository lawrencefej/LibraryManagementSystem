using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(DataContext context, IMapper mapper, ILogger<CategoryService> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Category> AddCategory(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<CategoryDto> AddCategory(CategoryDto categoryForCreation)
        {
            Category category = _mapper.Map<Category>(categoryForCreation);

            _context.Add(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Category {category} was added successfully");

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<LmsResponseHandler<CategoryDto>> DeleteCategory(int categoryId)
        {
            Category category = await GetCategory(categoryId);

            if (category != null)
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"{category} was deleted successfully");

                return LmsResponseHandler<CategoryDto>.Successful();
            }

            return LmsResponseHandler<CategoryDto>.Failed($"");
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            List<Category> categories = await _context.Categories.ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

            return category;
        }
    }
}
