using System.Threading.Tasks;
using LibraryManagementSystem.API.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Policy = Role.RequireLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryCardController : BaseApiController<LibraryCardForDetailedDto, LibrarycardForListDto>
    {
        private readonly ILibraryCardService _libraryCardService;

        public LibraryCardController(ILibraryCardService libraryCardService)
        {
            _libraryCardService = libraryCardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedCards([FromQuery] PaginationParams paginationParams)
        {
            PagedList<LibrarycardForListDto> cards = await _libraryCardService.GetAllLibraryCard(paginationParams);

            return ReturnPagination(cards);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard(LibraryCardForCreationDto addCardDto)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.AddLibraryCard(addCardDto);

            return CreatedAtRoute(nameof(GetById), new { cardId = result.Item.Id }, result.Item);
        }

        [HttpGet("{cardId}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(int cardId)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.GetLibraryCardById(cardId);

            return ResultCheck(result);
        }

        [HttpGet("cardnumber/{cardNumber}")]
        public async Task<IActionResult> GetLibraryCardByNumber(string cardNumber)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.GetLibraryCardByNumber(cardNumber);

            return ResultCheck(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(LibraryCardForUpdate updateCardDto)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.UpdateLibraryCard(updateCardDto);

            return ResultCheck(result);
        }

        [HttpDelete("{cardId}")]
        public async Task<IActionResult> Delete(int cardId)
        {
            LmsResponseHandler<LibraryCardForDetailedDto> result = await _libraryCardService.DeleteLibraryCard(cardId);

            return ResultCheck(result);
        }
    }
}
