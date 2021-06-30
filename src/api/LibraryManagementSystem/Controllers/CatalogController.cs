using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            // LibraryAsset assetForCreation = _mapper.Map<LibraryAsset>(libraryAssetForCreation);

            LibraryAssetForDetailedDto asset = await _libraryAssestService.AddAsset(libraryAssetForCreation);

            // _logger.LogInformation("User {0} added Asset {1} successfully", LoggedInUserID(), asset);

            // LibraryAssetForListDto assetToReturn = _mapper.Map<LibraryAssetForListDto>(asset);

            return CreatedAtRoute(nameof(GetLibraryAsset), new { assetId = asset.Id }, asset);
        }

        [HttpDelete("{assetId}")]
        public async Task<IActionResult> DeleteLibraryAsset(int assetId)
        {
            LibraryAsset asset = await _libraryAssestService.GetAsset(assetId);

            if (asset == null)
            {
                _logger.LogWarning("Asset {0} was not found", assetId);
                return BadRequest("Item not found");
            }
            await _libraryAssestService.DeleteAsset(asset);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            LibraryAsset asset = await _libraryAssestService.GetAsset(libraryAssetForUpdate.Id);

            if (asset == null)
            {
                _logger.LogWarning("Asset {0} was not found", libraryAssetForUpdate.Id);
                return BadRequest("Item not found");
            }

            _mapper.Map(libraryAssetForUpdate, asset);

            await _libraryAssestService.EditAsset(asset);

            return NoContent();
        }

        [HttpGet("{assetId}", Name = nameof(GetLibraryAsset))]
        public async Task<IActionResult> GetLibraryAsset(int assetId)
        {
            _logger.LogInformation("User {0} requested Asset {1}", LoggedInUserID(), assetId);
            LibraryAsset libraryAsset = await _libraryAssestService.GetAsset(assetId);

            if (libraryAsset == null)
            {
                _logger.LogWarning("Asset {0} was not found", assetId);
                return NoContent();
            }

            LibraryAssetForDetailedDto assetToReturn = _mapper.Map<LibraryAssetForDetailedDto>(libraryAsset);

            return Ok(assetToReturn);
        }

        [HttpGet("search/")]
        public async Task<IActionResult> SearchAvailableLibraryAsset([FromQuery] string searchString)
        {
            IEnumerable<LibraryAsset> assets = await _libraryAssestService.SearchAvalableLibraryAsset(searchString);

            IEnumerable<LibraryAssetForListDto> assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            return Ok(assetsToReturn);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetLibraryAssets([FromQuery] PaginationParams paginationParams)
        {
            PagedList<LibraryAsset> assets = await _libraryAssestService.GetAllAsync(paginationParams);

            IEnumerable<LibraryAssetForListDto> assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForListDto>>(assets);

            Response.AddPagination(assets.CurrentPage, assets.PageSize,
                 assets.TotalCount, assets.TotalPages);

            return Ok(assetsToReturn);
        }

        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetAssetForAuthor(int authorId)
        {
            IEnumerable<LibraryAsset> libraryAsset = await _libraryAssestService.GetAssetsByAuthor(authorId);

            if (libraryAsset == null)
            {
                return NoContent();
            }

            IEnumerable<LibraryAssetForDetailedDto> assetsToReturn = _mapper.Map<IEnumerable<LibraryAssetForDetailedDto>>(libraryAsset);

            return Ok(assetsToReturn);
        }

        // private bool IsCurrentuser(int id)
        // {
        //     if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //     {
        //         return false;
        //     }

        //     return true;
        // }

        private int LoggedInUserID()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return id;
        }
    }
}
