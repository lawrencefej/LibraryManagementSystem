using LMSLibrary.Dto;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.API.Controllers
{
    //[Route("api/catalog/{assetId}/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
    [Route("api/[controller]")]
    //[AllowAnonymous]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        // GET: api/Checkouts
        [HttpGet]
        public async Task<ActionResult> GetCheckouts()
        {
            var checkouts = await _checkoutService.GetAllCheckouts();

            return Ok(checkouts);
        }

        // GET: api/Checkouts/5
        [HttpGet("{id}", Name = "GetCheckout")]
        public async Task<ActionResult> GetCheckout(int id)
        {
            var checkout = await _checkoutService.GetCheckout(id);

            return Ok(checkout);
        }

        // PUT: api/Checkouts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> CheckInAsset(int id)
        {
            await _checkoutService.CheckInAsset(id);

            return NoContent();
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

        [HttpPost]
        public async Task<IActionResult> CheckoutAsset(CheckoutForCreationDto checkoutForCreationDto)
        {
            var result = await _checkoutService.CheckoutAsset(checkoutForCreationDto);

            if (!result.Valid)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}