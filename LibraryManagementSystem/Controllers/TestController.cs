using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSLibrary.Data;
using LMSLibrary.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : BaseController<TestController>
    {
        private readonly DataContext _context;

        public TestController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Test
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReserveAsset>>> GetReserveAssets()
        {
            Logger.LogError("Library asset was tested");
            return await _context.ReserveAssets.ToListAsync();
        }

        // GET: api/Test/5
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

        // PUT: api/Test/5
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

        // POST: api/Test
        [HttpPost]
        public async Task<ActionResult<ReserveAsset>> PostReserveAsset(ReserveAsset reserveAsset)
        {
            _context.ReserveAssets.Add(reserveAsset);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserveAsset", new { id = reserveAsset.Id }, reserveAsset);
        }

        // DELETE: api/Test/5
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
