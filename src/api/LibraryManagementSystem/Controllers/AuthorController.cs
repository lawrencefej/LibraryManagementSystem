using System.Threading.Tasks;
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
    public class AuthorController : BaseApiController<AuthorDto, AuthorDto>
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

            return ResultCheck(result);
        }

        [HttpPut]
        public async Task<IActionResult> EditAuthor(AuthorDto authorDto)
        {
            LmsResponseHandler<AuthorDto> result = await _authorService.EditAuthor(authorDto);

            return ResultCheck(result);
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

            return ResultCheck(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            PagedList<AuthorDto> authors = await _authorService.GetPaginatedAuthors(paginationParams);

            return ReturnPagination(authors);
        }
    }
}
