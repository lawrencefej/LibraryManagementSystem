using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LMSLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using LMSLibrary.Dto;
using AutoMapper;
using LMSService.Interfaces;

namespace LibraryManagement.API.Controllers
{
    //[Route("api/catalog/{assetId}/[controller]")]
    //[Authorize(Policy = "RequireLibrarianRole")]
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly ILibraryAssetRepository _libraryAssetRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly ILibraryCardRepository _libraryCardRepo;
        private readonly ILibraryRepository _libraryRepo;
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(ILibraryAssetRepository libraryAssetRepo,
            IMapper mapper, IUserRepository userRepo,
            ILibraryCardRepository libraryCardRepo, ILibraryRepository libraryRepo,
            ICheckoutService checkoutService)
        {
            _libraryAssetRepo = libraryAssetRepo;
            _mapper = mapper;
            _userRepo = userRepo;
            _libraryCardRepo = libraryCardRepo;
            _libraryRepo = libraryRepo;
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

        // DELETE: api/Checkouts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCheckout(int id)
        {
            //var checkout = await _context.Checkouts.FindAsync(id);
            //if (checkout == null)
            //{
            //    return NotFound();
            //}

            //_context.Checkouts.Remove(checkout);
            //await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
