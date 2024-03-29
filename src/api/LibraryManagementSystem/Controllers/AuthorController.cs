﻿using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
    [ApiController]
    public class AuthorController : BaseApiController<AuthorDto, AuthorDto>
    {
        private readonly IAuthorService _authorService;
        private readonly IDashboardService _dashboardService;

        public AuthorController(IAuthorService authorService, IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            _authorService = authorService;
        }

        [HttpGet("{authorId}", Name = nameof(GetAuthor))]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAuthor(int authorId)
        {
            LmsResponseHandler<AuthorDto> result = await _authorService.GetAuthorForController(authorId);

            return ResultCheck(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAuthor(AuthorDto authorDto)
        {
            LmsResponseHandler<AuthorDto> result = await _authorService.EditAuthor(authorDto);

            return ResultCheck(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAuthor(AuthorDto authorDto)
        {
            AuthorDto authorToReturn = await _authorService.AddAuthor(authorDto);

            await _dashboardService.BroadcastDashboardData();

            return CreatedAtRoute(nameof(GetAuthor), new { authorId = authorToReturn.Id }, authorToReturn);
        }

        [HttpDelete("{authorId}")]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            // TODO do not allow delete if author has books
            LmsResponseHandler<AuthorDto> result = await _authorService.DeleteAuthor(authorId);

            return ResultCheck(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            // TODO Fix pagination
            PagedList<AuthorDto> authors = await _authorService.GetPaginatedAuthors(paginationParams);

            return ReturnPagination(authors);
        }
    }
}
