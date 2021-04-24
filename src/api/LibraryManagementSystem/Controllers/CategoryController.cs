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
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var category = await categoryService.GetCategories();

            return Ok(category);
        }

        [HttpGet("{categoryId}", Name = nameof(GetCategory))]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            Category category = await categoryService.GetCategory(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            category = await categoryService.AddCategory(category);

            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            Category category = await categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            await categoryService.DeleteCategory(category);

            return category;
        }
    }
}
