using LMSLibrary.Dto;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/{userId}/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _reserveService;

        public ReserveController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }

        // GET: api/Reserve
        [Route("api/[controller]")]
        [HttpGet]
        [Authorize(Policy = "RequireLibrarianRole")]
        public async Task<ActionResult> GetReserveAssets()
        {
            var reserves = await _reserveService.GetAllReserves();

            return Ok(reserves);
        }

        [HttpGet]
        public async Task<ActionResult> GetReserveAssetsForMember(int userId)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var reserves = await _reserveService.GetReservesForMember(userId);

            return Ok(reserves);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetReserveAsset(int userId, int id)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var reserve = await _reserveService.GetReserveForMember(userId, id);

            return Ok(reserve);
        }

        // PUT: api/Reserve/5
        [HttpPut("{id}")]
        public async Task<IActionResult> CancelReserve(int userId, int id)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            await _reserveService.CancelReserve(userId, id);

            return NoContent();
        }

        // POST: api/Reserve
        [HttpPost]
        public async Task<ActionResult> ReserveAsset(int userId, ReserveForCreationDto reserveForCreation)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var result = await _reserveService.ReserveAsset(userId, reserveForCreation);

            if (!result.Valid)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("checkout/")]
        public async Task<IActionResult> GetCheckoutsForMember(int userId)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var checkouts = await _reserveService.GetCurrentCheckoutsForMember(userId);

            return Ok(checkouts);
        }

        private bool IsCurrentuser(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return false;
            }

            return true;
        }
    }
}