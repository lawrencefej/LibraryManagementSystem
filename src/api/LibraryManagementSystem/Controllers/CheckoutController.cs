using System.Threading.Tasks;
using LibraryManagementSystem.Controllers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> CheckInAsset(CheckoutForCheckInDto checkoutForCheckIn)
        {
            LmsResponseHandler<CheckoutForDetailedDto> result = await _checkoutService.CheckInAsset(checkoutForCheckIn);

            return ResultCheck(result);
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsset(Basket basketForCheckout)
        {
            LmsResponseHandler<CheckoutForDetailedDto> result = await _checkoutService.CheckoutAssets(basketForCheckout);

            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpGet("{id}", Name = nameof(GetCheckout))]
        public async Task<IActionResult> GetCheckout(int id)
        {
            LmsResponseHandler<CheckoutForDetailedDto> result = await _checkoutService.GetCheckoutWithDetails(id);

            return ResultCheck(result);
        }

        [HttpGet("asset/{libraryAssetId}")]
        public async Task<IActionResult> GetCheckoutsForAsset(int libraryAssetId, [FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetCurrentCheckoutsForAsset(libraryAssetId, paginationParams);

            return ReturnPagination(checkouts);
        }

        [HttpGet("asset/history/{libraryAssetId}")]
        public async Task<IActionResult> GetCheckoutHistoryForAsset(int libraryAssetId, [FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetCheckoutHistoryForAsset(libraryAssetId, paginationParams);

            return ReturnPagination(checkouts);
        }

        [HttpGet("card/{libraryCardId}")]
        public async Task<IActionResult> GetCurrentCheckoutsForCard(int libraryCardId, [FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetCurrentCheckoutsForCard(libraryCardId, paginationParams);

            return ReturnPagination(checkouts);
        }

        [HttpGet("card/history/{libraryCardId}")]
        public async Task<IActionResult> GetCheckoutHistoryForCard(int libraryCardId, [FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetCheckoutHistoryForCard(libraryCardId, paginationParams);

            return ReturnPagination(checkouts);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurrentCheckouts([FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetAllCurrentCheckouts(paginationParams);

            return ReturnPagination(checkouts);
        }

        [HttpGet("history/")]
        public async Task<IActionResult> GetCheckoutHistory([FromQuery] PaginationParams paginationParams)
        {
            PagedList<CheckoutForListDto> checkouts = await _checkoutService.GetCheckoutHistory(paginationParams);

            return ReturnPagination(checkouts);
        }
    }
}
