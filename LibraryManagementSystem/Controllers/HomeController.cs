using LMSRepository.Data;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILibraryAssetRepository _libraryAssetRepo;

        public HomeController(DataContext context, ILibraryAssetRepository libraryAssetRepo)
        {
            _context = context;
            _libraryAssetRepo = libraryAssetRepo;
        }

        // GET: api/Home
        [HttpGet]
        public IActionResult GetCheckouts()
        {
            var callbackUrl = new Uri(Request.Scheme + "://" + Request.Host);

            return Ok(callbackUrl);
        }

        // GET: api/Home/5
        [HttpGet("{id}")]
        public IActionResult GetCheckout(int id)
        {
            //var ip = Request.HttpContext.Connection.RemoteIpAddress;
            var ip = this.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            return Ok(ip);
        }

        // PUT: api/Home/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckout(int id, Checkout checkout)
        {
            if (id != checkout.Id)
            {
                return BadRequest();
            }

            _context.Entry(checkout).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckoutExists(id))
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

        // POST: api/Home
        [HttpPost]
        public async Task<ActionResult<Checkout>> PostCheckout(Checkout checkout)
        {
            _context.Checkouts.Add(checkout);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCheckout", new { id = checkout.Id }, checkout);
        }

        // DELETE: api/Home/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Checkout>> DeleteCheckout(int id)
        {
            var checkout = await _context.Checkouts.FindAsync(id);
            if (checkout == null)
            {
                return NotFound();
            }

            _context.Checkouts.Remove(checkout);
            await _context.SaveChangesAsync();

            return checkout;
        }

        private bool CheckoutExists(int id)
        {
            return _context.Checkouts.Any(e => e.Id == id);
        }
    }
}