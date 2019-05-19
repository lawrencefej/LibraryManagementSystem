using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSService.Dto;
using LMSService.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ITestService _testService;
        private readonly IMapper _mapper;

        public CatalogController(ILibraryAssestService libraryAssestService, ILogger<CatalogController> logger,
            ITestService testService, IMapper mapper)
        {
            _libraryAssestService = libraryAssestService;
            _logger = logger;
            _testService = testService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddLibraryAsset(LibraryAssetForCreationDto libraryAssetForCreation)
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
        [HttpGet("{assetId}", Name = nameof(GetLibraryAsset))]
        public async Task<IActionResult> GetLibraryAsset(int assetId)
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
        [EnableQuery()]
        public async Task<IActionResult> GetLibraryAssets()
        {
            var assets = await _libraryAssestService.GetAllAssets();

            return Ok(assets);
        }

        [AllowAnonymous]
        [HttpGet("pagination/")]
        public async Task<IActionResult> GetLibraryAssets([FromQuery]PaginationParams paginationParams)
        {
            var assets = await _testService.GetAllAssetsAsync(paginationParams);

            var assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            Response.AddPagination(assets.CurrentPage, assets.PageSize,
                 assets.TotalCount, assets.TotalPages);

            _logger.LogCritical("Test log");

            return Ok(assetsToReturn);
        }

        [AllowAnonymous]
        [HttpGet("odata/")]
        [EnableQuery]
        public IQueryable<LibraryAssetForListDto> GetAssets()
        {
            var assets = _libraryAssestService.GetAll();

            return assets;
        }

        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetAssetForAuthor(int authorId)
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