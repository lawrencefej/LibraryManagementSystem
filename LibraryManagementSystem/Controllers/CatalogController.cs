using LMSRepository.Dto;
using LMSService.Dto;
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

        [HttpPost]
        public async Task<ActionResult> AddLibraryAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            var asset = await _libraryAssestService.AddAsset(libraryAssetForCreation);

            //_logger.LogInformation($"{userId} added {libraryAssetForCreation.Title}");
            return CreatedAtRoute(nameof(GetLibraryAsset), new { assetId = asset.Id }, asset);
        }

        [HttpDelete("{assetId}")]
        public async Task<IActionResult> DeleteLibraryAsset(int userId, int assetId)
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

        [AllowAnonymous]
        //[HttpGet("{assetId}", Name = "GetAsset")]
        [HttpGet("{assetId}", Name = nameof(GetLibraryAsset))]
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
        [HttpGet("search/")]
        public async Task<IActionResult> SearchLibraryAsset([FromQuery]string searchString)
        {
            var assets = await _libraryAssestService.SearchLibraryAsset(searchString);

            return Ok(assets);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetLibraryAssets()
        {
            var assets = await _libraryAssestService.GetAllAssets();

            return Ok(assets);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult> GetAssetForAuthor(int authorId)
        {
            var libraryAsset = await _libraryAssestService.GetAssetsByAuthor(authorId);

            if (libraryAsset == null)
            {
                return NoContent();
            }

            return Ok(libraryAsset);
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