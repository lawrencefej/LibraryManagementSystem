using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorService authorService, IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public async Task<IActionResult> GetAuthor(int authorId)
        {
            var author = await _authorService.GetAuthor(authorId);

            if (author == null)
            {
                return NotFound();
            }

            var authorToReturn = _mapper.Map<AuthorDto>(author);

            return Ok(authorToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> EditAuthor(AuthorDto authorDto)
        {
            var author = await _authorService.GetAuthor(authorDto.Id);

            if (author == null)
            {
                return NotFound();
            }

            _mapper.Map(authorDto, author);

            await _authorService.EditAuthor(author);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);

            author = await _authorService.AddAuthor(author);

            var authorToReturn = _mapper.Map<AuthorDto>(author);

            return CreatedAtRoute("GetAuthor", new { authorId = author.Id }, authorToReturn);
        }

        [HttpDelete("{authorId}")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            var author = await _authorService.GetAuthor(authorId);

            if (author == null)
            {
                return NotFound();
            }

            await _authorService.DeleteAuthor(author);

            return NoContent();
        }

        [HttpGet("search/")]
        public async Task<IActionResult> SearchAuthors([FromQuery]string searchString)
        {
            var authors = await _authorService.SearchAuthors(searchString);

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return Ok(authorsToReturn);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetAll([FromQuery]PaginationParams paginationParams)
        {
            var authors = await _authorService.GetAllAsync(paginationParams);

            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            Response.AddPagination(authors.CurrentPage, authors.PageSize,
                 authors.TotalCount, authors.TotalPages);

            return Ok(authorsToReturn);
        }
    }
}