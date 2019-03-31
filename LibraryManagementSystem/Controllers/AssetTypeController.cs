using LMSRepository.Data;
using LMSRepository.Interfaces.Models;
using LMSService.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypeController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAssetTypeService _assetTypeService;

        public AssetTypeController(DataContext context, IAssetTypeService assetTypeService)
        {
            _context = context;
            _assetTypeService = assetTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetTypes()
        {
            var assetTypes = await _assetTypeService.GetAssetTypes();

            return Ok(assetTypes);
        }

        [HttpGet("{assetTypeId}")]
        public async Task<IActionResult> GetAssetType(int assetTypeId)
        {
            var assetType = await _assetTypeService.GetAssetType(assetTypeId);

            if (assetType == null)
            {
                return NotFound();
            }

            return Ok(assetType);
        }

        // PUT: api/AssetType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssetType(int id, AssetType assetType)
        {
            if (id != assetType.Id)
            {
                return BadRequest();
            }

            _context.Entry(assetType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetTypeExists(id))
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

        // POST: api/AssetType
        [HttpPost]
        public async Task<ActionResult<AssetType>> PostAssetType(AssetType assetType)
        {
            _context.AssetTypes.Add(assetType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssetType", new { id = assetType.Id }, assetType);
        }

        // DELETE: api/AssetType/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AssetType>> DeleteAssetType(int id)
        {
            var assetType = await _context.AssetTypes.FindAsync(id);
            if (assetType == null)
            {
                return NotFound();
            }

            _context.AssetTypes.Remove(assetType);
            await _context.SaveChangesAsync();

            return assetType;
        }

        private bool AssetTypeExists(int id)
        {
            return _context.AssetTypes.Any(e => e.Id == id);
        }
    }
}