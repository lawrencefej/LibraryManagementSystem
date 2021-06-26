using System.Collections.Generic;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
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
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            category = await _categoryService.AddCategory(category);

            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {

            if (await _categoryService.GetCategory(id) == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategory(await _categoryService.GetCategory(id));

            return await _categoryService.GetCategory(id);
        }
    }
}
