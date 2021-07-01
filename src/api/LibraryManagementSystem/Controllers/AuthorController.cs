using System.Threading.Tasks;
using LibraryManagementSystem.API.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{authorId}", Name = nameof(GetAuthor))]
        public async Task<IActionResult> GetAuthor(int authorId)
        {
            LmsResponseHandler<AuthorDto> result = await _authorService.GetAuthorForController(authorId);

            return result.Succeeded ? Ok(result.Item) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> EditAuthor(AuthorDto authorDto)
        {
            LmsResponseHandler<AuthorDto> result = await _authorService.EditAuthor(authorDto);

            return result.Succeeded ? NoContent() : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorDto authorDto)
        {
            AuthorDto authorToReturn = await _authorService.AddAuthor(authorDto);

            return CreatedAtRoute(nameof(GetAuthor), new { authorId = authorToReturn.Id }, authorToReturn);
        }

        [HttpDelete("{authorId}")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            LmsResponseHandler<AuthorDto> result = await _authorService.DeleteAuthor(authorId);

            return result.Succeeded ? NoContent() : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            PagedList<AuthorDto> authors = await _authorService.GetPaginatedAuthors(paginationParams);

            Response.AddPagination(authors.CurrentPage, authors.PageSize,
                 authors.TotalCount, authors.TotalPages);

            return Ok(authors);
        }
    }
}
