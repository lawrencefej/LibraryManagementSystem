using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ILibraryAssetService _libraryAssestService;
        private readonly ILogger<CatalogController> _logger;
        private readonly IMapper _mapper;

        public CatalogController(ILibraryAssetService libraryAssestService, ILogger<CatalogController> logger,
            IMapper mapper)
        {
            _libraryAssestService = libraryAssestService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddLibraryAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            var assetForCreation = _mapper.Map<LibraryAsset>(libraryAssetForCreation);

            var asset = await _libraryAssestService.AddAsset(assetForCreation);

            var assetToReturn = _mapper.Map<LibraryAssetForListDto>(asset);

            return CreatedAtRoute(nameof(GetLibraryAsset), new { assetId = asset.Id }, assetToReturn);
        }

        [HttpDelete("{assetId}")]
        public async Task<IActionResult> DeleteLibraryAsset(int assetId)
        {
            await _libraryAssestService.DeleteAsset(assetId);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            var asset = await _libraryAssestService.GetAsset(libraryAssetForUpdate.Id);

            if (asset == null)
            {
                return BadRequest("Item not found");
            }

            _mapper.Map(libraryAssetForUpdate, asset);

            await _libraryAssestService.EditAsset(asset);

            return NoContent();
        }

        [HttpGet("{assetId}", Name = nameof(GetLibraryAsset))]
        public async Task<IActionResult> GetLibraryAsset(int assetId)
        {
            // TODO decide if you want this
            var userId = LoggedInUserID();
            _logger.LogInformation("User {0} requested Asset {1}", userId, assetId);
            var libraryAsset = await _libraryAssestService.GetAsset(assetId);

            if (libraryAsset == null)
            {
                _logger.LogWarning("Asset {0} was not found", assetId);
                return NoContent();
            }

            var assetToReturn = _mapper.Map<LibraryAssetForDetailedDto>(libraryAsset);

            return Ok(assetToReturn);
        }

        [HttpGet("search/")]
        public async Task<IActionResult> SearchLibraryAsset([FromQuery]string searchString)
        {
            var assets = await _libraryAssestService.SearchLibraryAsset(searchString);

            var assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            return Ok(assetsToReturn);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetLibraryAssets([FromQuery]PaginationParams paginationParams)
        {
            var assets = await _libraryAssestService.GetAllAsync(paginationParams);

            var assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            Response.AddPagination(assets.CurrentPage, assets.PageSize,
                 assets.TotalCount, assets.TotalPages);

            return Ok(assetsToReturn);
        }

        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetAssetForAuthor(int authorId)
        {
            var libraryAsset = await _libraryAssestService.GetAssetsByAuthor(authorId);

            if (libraryAsset == null)
            {
                return NoContent();
            }

            var assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(libraryAsset);

            return Ok(assetsToReturn);
        }

        private bool IsCurrentuser(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return false;
            }

            return true;
        }

        private int LoggedInUserID()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return id;
        }
    }
}