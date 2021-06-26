using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
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
            PagedList<LibraryCard> cards = await _libraryCardService.GetAllLibraryCard(paginationParams);

            IEnumerable<LibrarycardForListDto> usersToReturn = _mapper.Map<IEnumerable<LibrarycardForListDto>>(cards);

            return Ok(usersToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard(LibraryCardForCreationDto addCardDto)
        {

            LibraryCard cardForCreation = _mapper.Map<LibraryCard>(addCardDto);

            LibraryCard card = await _libraryCardService.AddLibraryCard(cardForCreation);

            _logger.LogInformation("User {0} added Asset {1} successfully", LoggedInUserID(), card);

            LibraryCardForDetailedDto cardToReturn = _mapper.Map<LibraryCardForDetailedDto>(card);

            return CreatedAtRoute(nameof(GetById), new { cardId = card.Id }, cardToReturn);
        }

        [HttpGet("{cardId}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(int cardId)
        {
            LibraryCard card = await _libraryCardService.GetLibraryCardById(cardId);

            if (card == null)
            {
                _logger.LogWarning("card {0} was not found", cardId);
                return NoContent();
            }

            LibraryCardForDetailedDto cardToReturn = _mapper.Map<LibraryCardForDetailedDto>(card);

            return Ok(cardToReturn);
        }

        [HttpGet("cardnumber/{cardNumber}")]
        public async Task<IActionResult> GetLibraryCardByNumber(string cardNumber)
        {
            LibraryCard card = await _libraryCardService.GetLibraryCardByNumber(cardNumber);

            if (card == null)
            {
                _logger.LogWarning("card {0} was not found", cardNumber);
                return NoContent();
            }

            LibraryCardForDetailedDto cardToReturn = _mapper.Map<LibraryCardForDetailedDto>(card);

            return Ok(cardToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> Put(LibraryCardForUpdate updateCardDto)
        {
            LibraryCard card = await _libraryCardService.GetLibraryCardById(updateCardDto.Id);

            if (card == null)
            {
                return BadRequest("User for not found");
            }

            await _libraryCardService.UpdateLibraryCard(card);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            LibraryCard card = await _libraryCardService.GetLibraryCardById(id);

            if (card == null)
            {
                NoContent();
            }

            await _libraryCardService.DeleteLibraryCard(card);

            return NoContent();
        }

        private int LoggedInUserID()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return id;
        }
    }
}
