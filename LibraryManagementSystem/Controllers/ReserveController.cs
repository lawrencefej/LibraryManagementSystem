using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSLibrary.Data;
using LMSLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using LMSService.Interfaces;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ReserveController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IReserveService _reserveService;

        public ReserveController(DataContext context, IReserveService reserveService)
        {
            _context = context;
            _reserveService = reserveService;
        }

        // GET: api/Reserve
        [HttpGet]
        public async Task<ActionResult> GetReserveAssets()
        {
            var reserves = await _reserveService.GetAllReserves();

            return Ok(reserves);
        }

        // GET: api/Reserve/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReserveAsset>> GetReserveAsset(int id)
        {
            var reserveAsset = await _context.ReserveAssets.FindAsync(id);

            if (reserveAsset == null)
            {
                return NotFound();
            }

            return reserveAsset;
        }

        // PUT: api/Reserve/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserveAsset(int id, ReserveAsset reserveAsset)
        {
            if (id != reserveAsset.Id)
            {
                return BadRequest();
            }

            _context.Entry(reserveAsset).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReserveAssetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reserve
        [HttpPost]
        public async Task<ActionResult<ReserveAsset>> PostReserveAsset(ReserveAsset reserveAsset)
        {
            _context.ReserveAssets.Add(reserveAsset);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserveAsset", new { id = reserveAsset.Id }, reserveAsset);
        }

        // DELETE: api/Reserve/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReserveAsset>> DeleteReserveAsset(int id)
        {
            var reserveAsset = await _context.ReserveAssets.FindAsync(id);
            if (reserveAsset == null)
            {
                return NotFound();
            }

            _context.ReserveAssets.Remove(reserveAsset);
            await _context.SaveChangesAsync();

            return reserveAsset;
        }

        private bool ReserveAssetExists(int id)
        {
            return _context.ReserveAssets.Any(e => e.Id == id);
        }
    }
}
