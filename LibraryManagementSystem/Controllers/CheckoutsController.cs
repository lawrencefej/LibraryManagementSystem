using LMSLibrary.Dto;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Controllers
{
    [Authorize(Policy = "RequireLibrarianRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        // PUT: api/Checkouts/5
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

            if (!result.Valid)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtRoute("GetCheckout", new { id = result.Id }, result.Result);

            //return NoContent();
        }

        [HttpPut("reserve/{id}")]
        public async Task<IActionResult> CheckOutReserve(int id)
        {
            var checkout = await _checkoutService.CheckoutReservedAsset(id);

            if (checkout.Valid)
            {
                return NoContent();
            }

            return BadRequest();
        }

        // GET: api/Checkouts/5
        [HttpGet("{id}", Name = "GetCheckout")]
        public async Task<ActionResult> GetCheckout(int id)
        {
            var checkout = await _checkoutService.GetCheckout(id);

            return Ok(checkout);
        }

        // GET: api/Checkouts
        [HttpGet]
        public async Task<ActionResult> GetCheckouts()
        {
            var checkouts = await _checkoutService.GetAllCheckouts();

            return Ok(checkouts);
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
    }
}