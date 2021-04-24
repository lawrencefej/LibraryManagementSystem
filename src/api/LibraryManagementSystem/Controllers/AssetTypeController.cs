using System.Collections.Generic;
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
        private readonly IAssetTypeService assetTypeService;

        public AssetTypeController(IAssetTypeService assetTypeService)
        {
            this.assetTypeService = assetTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetTypes()
        {
            IEnumerable<AssetType> assetTypes = await assetTypeService.GetAssetTypes();

            return Ok(assetTypes);
        }

        [HttpGet("{assetTypeId}", Name = nameof(GetAssetType))]
        public async Task<IActionResult> GetAssetType(int assetTypeId)
        {
            AssetType assetType = await assetTypeService.GetAssetType(assetTypeId);

            if (assetType == null)
            {
                return NotFound();
            }

            return Ok(assetType);
        }

        [HttpPost]
        public async Task<IActionResult> AddAssetType(AssetType assetType)
        {
            assetType = await assetTypeService.AddAssetType(assetType);

            return CreatedAtAction(nameof(GetAssetType), new { assetTypeId = assetType.Id }, assetType);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AssetType>> DeleteAssetType(int id)
        {
            AssetType assetType = await assetTypeService.GetAssetType(id);

            if (assetType == null)
            {
                return NotFound();
            }

            await assetTypeService.DeleteAssetType(assetType);

            return NoContent();
        }
    }
}
