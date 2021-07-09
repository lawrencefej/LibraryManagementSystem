using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Policy = Role.RequireLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryCardController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILibraryCardService _libraryCardService;
        private readonly ILogger<LibraryCardController> _logger;
        public LibraryCardController(IMapper mapper, ILibraryCardService libraryCardService, ILogger<LibraryCardController> logger)
        {
            _logger = logger;
            _libraryCardService = libraryCardService;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedCards([FromQuery] PaginationParams paginationParams)
        {
            PagedList<LibrarycardForListDto> cards = await _libraryCardService.GetAllLibraryCard(paginationParams);

            Response.AddPagination(cards.CurrentPage, cards.PageSize,
                 cards.TotalCount, cards.TotalPages);

            return Ok(cards);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard(LibraryCardForCreationDto addCardDto)
        {
            LibraryCardForDetailedDto cardToReturn = await _libraryCardService.AddLibraryCard(addCardDto);

            return CreatedAtRoute(nameof(GetById), new { cardId = cardToReturn.Id }, cardToReturn);
        }

        [HttpGet("{cardId}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(int cardId)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.GetLibraryCardById(cardId);

            return result.Succeeded ? Ok(result.Item) : NotFound();
        }

        [HttpGet("cardnumber/{cardNumber}")]
        public async Task<IActionResult> GetLibraryCardByNumber(string cardNumber)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.GetLibraryCardByNumber(cardNumber);

            return result.Succeeded ? Ok(result.Item) : NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Put(LibraryCardForUpdate updateCardDto)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.UpdateLibraryCard(updateCardDto);

            return ResultCheck(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(LibraryCardForDetailedDto cardForDel)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.DeleteLibraryCard(cardForDel);

            return ResultCheck(result);
        }

        private IActionResult ResultCheck(LmsResponseHandler<LibraryCardForDetailedDto> result)
        {
            return result.Succeeded ? NoContent() : NotFound(result.Error);
        }
    }
}
