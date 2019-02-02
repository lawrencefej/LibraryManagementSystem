using LMSRepository.Dto;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ILibraryAssestService _libraryAssestService;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ILibraryAssestService libraryAssestService, ILogger<CatalogController> logger)
        {
            _libraryAssestService = libraryAssestService;
            _logger = logger;
        }

        // DELETE: api/Catalog/5
        [HttpPost("{userId}")]
        public async Task<ActionResult> AddLibraryAsset(int userId, LibraryAssetForCreationDto libraryAssetForCreation)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            await _libraryAssestService.AddAsset(libraryAssetForCreation);

            _logger.LogInformation($"{userId} added {libraryAssetForCreation.Title}");

            return NoContent();
        }

        [HttpDelete("{assetId}")]
        public async Task<IActionResult> DeleteLibararyAsset(int userId, int assetId)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            await _libraryAssestService.DeleteAsset(assetId);

            return NoContent();
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> EditAsset(int userId, LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            await _libraryAssestService.EditAsset(libraryAssetForUpdate);

            return NoContent();
        }

        // GET: api/Catalog/5
        [AllowAnonymous]
        [HttpGet("{assetId}")]
        public async Task<ActionResult> GetLibraryAsset(int assetId)
        {
            var libraryAsset = await _libraryAssestService.GetAsset(assetId);

            if (libraryAsset == null)
            {
                return NoContent();
            }

            return Ok(libraryAsset);
        }

        [AllowAnonymous]
        [HttpGet("search/{searchString}")]
        public async Task<IActionResult> SearchLibraryAsset(string searchString)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var assets = await _libraryAssestService.SearchLibraryAsset(searchString);

                return Ok(assets);
            }

            return Ok();
        }

        // GET: api/Catalog
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetLibraryAssets()
        {
            var assets = await _libraryAssestService.GetAllAssets();

            return Ok(assets);
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