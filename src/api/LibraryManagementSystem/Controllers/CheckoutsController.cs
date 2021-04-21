using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
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

        //[HttpPost]
        //public async Task<IActionResult> CheckoutAsset(CheckoutForCreationDto checkoutForCreationDto)
        //{
        //    var result = await _checkoutService.CheckoutAsset(checkoutForCreationDto);

        //    return CreatedAtRoute("GetCheckout", new { id = result.Id }, result);
        //}

        [HttpPost]
        public async Task<IActionResult> CheckoutAsset(IEnumerable<CheckoutForCreationDto> checkoutForCreationDto)
        {
            //var result = await _checkoutService.CheckoutAsset(checkoutForCreationDto);

            //return CreatedAtRoute("GetCheckout", new { id = result.Id }, result);

            await _checkoutService.CheckoutAsset(checkoutForCreationDto);

            return Ok();
        }

        [HttpGet("{id}", Name = "GetCheckout")]
        public async Task<ActionResult> GetCheckout(int id)
        {
            var checkout = await _checkoutService.GetCheckout(id);

            if (checkout == null)
            {
                return NotFound();
            }

            var checkoutToReturn = _mapper.Map<CheckoutForReturnDto>(checkout);

            return Ok(checkoutToReturn);
        }

        [HttpGet("asset/{libraryAssetId}")]
        public async Task<ActionResult> GetCheckoutsForAsset(int libraryAssetId)
        {
            var checkouts = await _checkoutService.GetCheckoutsForAsset(libraryAssetId);

            var checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            return Ok(checkoutsToReturn);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetCheckoutsForMember(int userId)
        {
            var checkouts = await _checkoutService.GetCheckoutsForMember(userId);

            var checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            return Ok(checkoutsToReturn);
        }

        [HttpGet("search/")]
        public async Task<IActionResult> SearchCheckouts([FromQuery] string searchString)
        {
            var checkouts = await _checkoutService.SearchCheckouts(searchString);

            var checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            return Ok(checkoutsToReturn);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var checkouts = await _checkoutService.GetAllCurrentCheckouts(paginationParams);

            var checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            Response.AddPagination(checkouts.CurrentPage, checkouts.PageSize,
                 checkouts.TotalCount, checkouts.TotalPages);

            return Ok(checkoutsToReturn);
        }
    }
}
