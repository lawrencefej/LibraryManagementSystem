using LMSRepository.Dto;
using LMSService.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorService.GetAuthors();

            return Ok(authors);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public async Task<IActionResult> GetAuthor(int authorId)
        {
            var author = await _authorService.GetAuthor(authorId);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPut]
        public async Task<IActionResult> EditAuthor(AuthorDto authorDto)
        {
            await _authorService.EditAuthor(authorDto);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorDto authorDto)
        {
            var author = await _authorService.AddAuthor(authorDto);

            return CreatedAtRoute("GetAuthor", new { authorId = author.Id }, author);
        }

        [HttpDelete("{authorId}")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            await _authorService.DeleteAuthor(authorId);

            return NoContent();
        }

        [HttpGet("search/")]
        public async Task<IActionResult> SearchAuthors([FromQuery]string searchString)
        {
            var assets = await _authorService.SearchAuthors(searchString);

            return Ok(assets);
        }
    }
}