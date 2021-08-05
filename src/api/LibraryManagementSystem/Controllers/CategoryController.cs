using System.Collections.Generic;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            IEnumerable<Category> category = await _categoryService.GetCategories();

            return Ok(category);
        }

        [HttpGet("{categoryId}", Name = nameof(GetCategory))]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            Category category = await _categoryService.GetCategory(categoryId);

            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(CategoryDto categoryForCreation)
        {
            CategoryDto category = await _categoryService.AddCategory(categoryForCreation);

            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            // TODO do not allow delete if category is assigned to assets
            await _categoryService.DeleteCategory(id);

            return await _categoryService.GetCategory(id);
        }
    }
}
