using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Interfaces.Models;
using LMSService.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ICategoryService _categoryService;

        public CategoryController(DataContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            //var test = await _context.Checkouts
            //    .Where(d => d.Since > DateTime.Today.AddMonths(-1))
            //    .GroupBy(d => new { day = d.Since })
            //    .Select(x => new { count = x.Count(), day = x.Key.day })
            //    .ToListAsync();

            //var test = await _context.LibraryAssets
            //    .GroupBy(d => d.AssetType.Name)
            //    .Select(x => new { Typee = x.Key, TypeCount = x.Count() })
            //    .ToListAsync();

            //var test = new TotalsReport();

            //test.TotalItems = await _context.LibraryAssets.CountAsync();
            //test.TotalMembers = _context.Users.Count();
            //test.TotalCheckouts = _context.Checkouts.Count();
            //test.TotalAuthors = _context.Authors.Count();

            //return Ok(test);
            var category = await _categoryService.GetCategories();

            return Ok(category);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Category>> GetCategory(int categoryId)
        {
            var category = await _categoryService.GetCategory(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}