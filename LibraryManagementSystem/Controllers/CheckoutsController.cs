using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var result = await _checkoutService.CheckoutAsset(checkoutForCreationDto);

            if (!result.IsSuccessful)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtRoute("GetCheckout", new { id = result.Id }, result.Result);
        }

        [HttpGet("{id}", Name = "GetCheckout")]
        public async Task<ActionResult> GetCheckout(int id)
        {
            var checkout = await _checkoutService.GetCheckout(id);

            return Ok(checkout);
        }

        [HttpGet("asset/{libraryAssetId}")]
        public async Task<ActionResult> GetCheckoutsForAsset(int libraryAssetId)
        {
            var checkouts = await _checkoutService.GetCheckoutsForAsset(libraryAssetId);

            return Ok(checkouts);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetCheckoutsForMember(int userId)
        {
            var checkouts = await _checkoutService.GetCheckoutsForMember(userId);

            return Ok(checkouts);
        }

        [HttpGet("search/")]
        public async Task<IActionResult> SearchCheckouts([FromQuery]string searchString)
        {
            var checkouts = await _checkoutService.SearchCheckouts(searchString);

            return Ok(checkouts);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetAll([FromQuery]PaginationParams paginationParams)
        {
            var checkouts = await _checkoutService.GetAllCheckouts(paginationParams);

            var checkoutsToReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouts);

            Response.AddPagination(checkouts.CurrentPage, checkouts.PageSize,
                 checkouts.TotalCount, checkouts.TotalPages);

            return Ok(checkoutsToReturn);
        }
    }
}