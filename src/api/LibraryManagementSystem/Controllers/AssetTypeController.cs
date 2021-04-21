using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypeController : ControllerBase
    {
        private readonly IAssetTypeService _assetTypeService;

        public AssetTypeController(IAssetTypeService assetTypeService)
        {
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

        [HttpPost]
        public async Task<IActionResult> AddAssetType(AssetType assetType)
        {
            assetType = await _assetTypeService.AddAssetType(assetType);

            return CreatedAtAction("GetAssetType", new { id = assetType.Id }, assetType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AssetType>> DeleteAssetType(int id)
        {
            var assetType = await _assetTypeService.GetAssetType(id);

            if (assetType == null)
            {
                return NotFound();
            }

            await _assetTypeService.DeleteAssetType(assetType);

            return NoContent();
        }
    }
}
