using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSLibrary.Data;
using LMSLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using LMSLibrary.Dto;
using AutoMapper;
using FluentValidation;
using LMSLibrary.Validators;
using LMSLibrary.Services;
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
        private readonly DataContext _context;
        private readonly ILibraryAssetRepository _libraryAssetRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly ILibraryCardRepository _libraryCardRepo;
        private readonly ILibraryRepository _libraryRepo;
        private readonly ICheckoutService _checkoutService;

        public CheckoutsController(DataContext context,
            ILibraryAssetRepository libraryAssetRepo,
            IMapper mapper,
            IUserRepository userRepo,
            ILibraryCardRepository libraryCardRepo,
            ILibraryRepository libraryRepo,
            ICheckoutService checkoutService)
        {
            _context = context;
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
            //var checkouts = await _.
            var checkouts = await _context.Checkouts.ToListAsync();

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
            var reserve = await _libraryRepo.GetReserve(id);

            if (reserve == null)
            {
                return NotFound();
            }

            if (reserve.Status.Name == "Checkedout")
            {
                return BadRequest($"{reserve.Id} has already been checked out");
            }

            if (reserve.Status.Name == "Expired")
            {
                return BadRequest($"{reserve.Id} has expired");
            }

            reserve.Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == 3);
            reserve.DateCheckedOut = DateTime.Now;

            var checkout = new CheckoutForCreationDto()
            {
                LibraryAssetId = reserve.LibraryAssetId,
                LibraryCardId = reserve.LibraryCardId
            };

            var result = await _checkoutService.CheckoutReservedAsset(reserve);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutAsset(CheckoutForCreationDto checkoutForCreationDto)
        {
            var result = await _checkoutService.CheckoutAsset(checkoutForCreationDto);

            if (!result.Valid)
            {
                return BadRequest(result.Errors);
            }

            //return Ok(result.Result);

            //var checkout = await _checkoutService.CreateCheckout(checkoutForCreationDto);

            return CreatedAtRoute(nameof(GetCheckout), new { id = result.Id }, result.Result);
        }

        // DELETE: api/Checkouts/5
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
