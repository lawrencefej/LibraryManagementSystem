using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSLibrary.Data;
using LMSLibrary.Models;
using AutoMapper;
using LMSLibrary.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace LibraryManagement.API.Controllers
{

    [Route("api/[controller]")]
    //[AllowAnonymous]
    [Authorize(Policy = "RequireMemberRole")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILibraryAssetRepository _libraryAssetRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public CatalogController(DataContext context, 
            ILibraryAssetRepository libraryAssetRepo,
            IUserRepository userRepo,
            IMapper mapper)
        {
            _context = context;
            _libraryAssetRepo = libraryAssetRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        // GET: api/Catalog
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetLibraryAssets()
        {
            var assets = await _libraryAssetRepo.GetLibraryAssets();

            var assetToReturn = _mapper.Map<IEnumerable<LibraryAssetForDetailedDto>>(assets);

            return Ok(assetToReturn);
        }

        // GET: api/Catalog/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetLibraryAsset(int id)
        {
            var libraryAsset = await _libraryAssetRepo.GetAsset(id);

            if (libraryAsset == null)
            {
                return NoContent();
            }

            var assetToReturn = _mapper.Map<LibraryAssetForDetailedDto>(libraryAsset);

            return Ok(assetToReturn);
        }

        // PUT: api/Catalog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibraryAsset(int id, LibraryAsset libraryAsset)
        {
            if (id != libraryAsset.Id)
            {
                return BadRequest();
            }

            _context.Entry(libraryAsset).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryAssetExists(id))
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

        //[HttpPost("{assetId}/reserveasset/{id}")]
        //public async Task<IActionResult> ReserveAsset(int assetId, int id)
        //{

        //    if (!IsCurrentuser(id))
        //    {
        //        return Unauthorized();
        //    }

        //    var libraryCard = await _userRepo.GetUserLibraryCard(id);
        //    var libraryAsset = await _libraryAssetRepo.GetAsset(assetId);

        //    var errors = _libraryAssetRepo.ValidateCheckout(libraryAsset, libraryCard);

        //    if (errors.Any())
        //    {
        //        return BadRequest(errors);
        //    }

        //    var reserveForCreation = new ReserveForCreationDto()
        //    {
        //        LibraryAssetId = libraryAsset.Id,
        //        LibraryCardId = libraryCard.Id,
        //        //Status2 = libraryAsset.Status.Name,
        //        Fees = libraryCard.Fees
        //    };

        //    var context = new ValidationContext(reserveForCreation);
        //    var results = new List<ValidationResult>();
        //    var isValid = Validator.TryValidateObject(reserveForCreation, context, results);

        //    if (!isValid)
        //    {
        //        return BadRequest(results);
        //    }

        //    var reserve = _mapper.Map<ReserveAsset>(reserveForCreation);

        //    reserve.Status = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == 4);

        //    _libraryAssetRepo.Add(reserve);

        //    _libraryAssetRepo.ReduceAssetCopiesAvailable(libraryAsset); 

        //    var reserveToReturn = _mapper.Map<ReserveForReturnDto>(reserve);


        //    if (await _libraryAssetRepo.SaveAll())
        //    {
        //        //return CreatedAtRoute("GetCheckout", new { id = reserve.Id }, reserveToReturn);
        //        reserveToReturn.Id = reserve.Id;
        //        return Ok(reserveToReturn);
        //    }

        //    return BadRequest("Failed to Checkout the item");

        //}

        private bool Test(ReserveForCreationDto reserveForCreation)
        {
            return true;
        }

        // DELETE: api/Catalog/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelReservation(int id, int userId)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var libraryAsset = await _context.LibraryAssets.FindAsync(userId);

            if (libraryAsset == null)
            {
                return NotFound();
            }

            _context.LibraryAssets.Remove(libraryAsset);

            await _context.SaveChangesAsync();

            return Ok(libraryAsset);
        } 

        private bool LibraryAssetExists(int id)
        {
            return _context.LibraryAssets.Any(e => e.Id == id);
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
