using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Extensions;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.API.Controllers
{
    [Authorize(Policy = "RequireLibrarianRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutsController> _logger;

        public CheckoutsController(ICheckoutService checkoutService, IMapper mapper, ILogger<CheckoutsController> logger)
        {
            _checkoutService = checkoutService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> CheckInAsset(int id)
        {
            await _checkoutService.CheckInAsset(id);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsset(CheckoutForCreationDto checkoutForCreationDto)
        {
            LibraryCard card = await _checkoutService.GetMemberLibraryCard(checkoutForCreationDto.LibraryCardNumber);

            if (card == null)
            {
                return BadRequest("LibraryCard Does Not Exist");
            }

            LmsResponseHandler<CheckoutForReturnDto> result = await _checkoutService.CheckoutItems(card, checkoutForCreationDto);

            return result.Succeeded ? CreatedAtRoute("GetCheckout", new { id = result.Item.Id }, result) : BadRequest(result.Error);
        }

        // [HttpPost]
        // public async Task<IActionResult> CheckoutAsset(IEnumerable<CheckoutForCreationDto> checkoutForCreationDto)
        // {

        //     //var result = await _checkoutService.CheckoutAsset(checkoutForCreationDto);

        //     //return CreatedAtRoute("GetCheckout", new { id = result.Id }, result);

        //     await _checkoutService.CheckoutAsset(checkoutForCreationDto);

        //     return Ok();
        // }

        [HttpGet("{id}", Name = "GetCheckout")]
        public async Task<ActionResult> GetCheckout(int id)
        {
            Checkout checkout = await _checkoutService.GetCheckout(id);

            if (checkout == null)
            {
                return NotFound();
            }

            CheckoutForReturnDto checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);

            return Ok(checkoutToReturn);
        }

        [HttpGet("asset/{libraryAssetId}")]
        public async Task<ActionResult> GetCheckoutsForAsset(int libraryAssetId)
        {
            IEnumerable<Checkout> checkouts = await _checkoutService.GetCheckoutsForAsset(libraryAssetId);

            IEnumerable<CheckoutForReturnDto> checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            return Ok(checkoutsToReturn);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetCheckoutsForMember(int userId)
        {
            IEnumerable<Checkout> checkouts = await _checkoutService.GetCheckoutsForMember(userId);

            IEnumerable<CheckoutForReturnDto> checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            return Ok(checkoutsToReturn);
        }

        [HttpGet("search/")]
        public async Task<IActionResult> SearchCheckouts([FromQuery] string searchString)
        {
            IEnumerable<Checkout> checkouts = await _checkoutService.SearchCheckouts(searchString);

            IEnumerable<CheckoutForReturnDto> checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            return Ok(checkoutsToReturn);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            PagedList<Checkout> checkouts = await _checkoutService.GetAllCurrentCheckouts(paginationParams);

            IEnumerable<CheckoutForReturnDto> checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            Response.AddPagination(checkouts.CurrentPage, checkouts.PageSize,
                 checkouts.TotalCount, checkouts.TotalPages);

            return Ok(checkoutsToReturn);
        }
    }
}
