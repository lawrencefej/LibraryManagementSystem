using System.Threading.Tasks;
using LibraryManagementSystem.Controllers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Authorize(Policy = "RequireLibrarianRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : BaseApiController<CheckoutForDetailedDto, CheckoutForListDto>
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckInAsset(CheckoutForCheckInDto checkoutForCheckIn)
        {
            LmsResponseHandler<CheckoutForDetailedDto> result = await _checkoutService.CheckInAsset(checkoutForCheckIn);

            return ResultCheck(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckoutAsset(BasketForCheckoutDto basketForCheckout)
        {
            LmsResponseHandler<CheckoutForDetailedDto> result = await _checkoutService.CheckoutAssets(basketForCheckout);

            return ResultCheck(result);
        }

        [HttpGet("{id}", Name = nameof(GetCheckout))]
        [ProducesResponseType(typeof(CheckoutForDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCheckout(int id)
        {
            LmsResponseHandler<CheckoutForDetailedDto> result = await _checkoutService.GetCheckoutWithDetails(id);

            return ResultCheck(result);
        }

        [HttpGet("asset/{libraryAssetId}")]
        [ProducesResponseType(typeof(CheckoutForListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCheckoutsForAsset(int libraryAssetId, [FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetCheckoutsForAsset(libraryAssetId, paginationParams);

            return ReturnPagination(checkouts);
        }

        [HttpGet("card/{libraryCardId}")]
        [ProducesResponseType(typeof(CheckoutForListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentCheckoutsForCard(int libraryCardId, [FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetCheckoutsForCard(libraryCardId, paginationParams);

            return ReturnPagination(checkouts);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CheckoutForListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCheckouts([FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetCheckouts(paginationParams);

            return ReturnPagination(checkouts);
        }
    }
}
