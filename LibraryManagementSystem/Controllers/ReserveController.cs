using LibraryManagementSystem.API.Helpers;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    [Route("api/{userId}/[controller]")]
    [ApiController]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _reserveService;

        public ReserveController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }

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

        [Route("api/[controller]")]
        [HttpGet]
        [Authorize(Policy = Role.RequireLibrarianRole)]
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
        [HttpPost("{assetId}")]
        public async Task<ActionResult> ReserveAsset(int userId, int assetId)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var result = await _reserveService.ReserveAsset(userId, assetId);

            if (!result.Valid)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
        private bool IsCurrentuser(int id)
        {
            var currentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != currentUser && !(User.IsInRole(Role.Librarian) || User.IsInRole(Role.Admin)))
            {
                return false;
            }

            return true;
        }
    }
}