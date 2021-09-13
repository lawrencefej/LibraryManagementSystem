using System.Threading.Tasks;
using LibraryManagementSystem.API.Helpers;
using LibraryManagementSystem.Controllers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = Role.RequireLibrarianRole)]
    [ApiController]
    public class CatalogController : BaseApiController<LibraryAssetForDetailedDto, LibraryAssetForListDto>
    {
        private readonly ILibraryAssetService _libraryAssestService;
        private readonly IDashboardService _dashboardService;

        public CatalogController(ILibraryAssetService libraryAssestService, IDashboardService dashboardService)
        {
            _libraryAssestService = libraryAssestService;
            _dashboardService = dashboardService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LibraryAssetForDetailedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLibraryAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            LibraryAssetForDetailedDto asset = await _libraryAssestService.AddAsset(libraryAssetForCreation);

            await _dashboardService.BroadcastDashboardData();

            return CreatedAtRoute(nameof(GetLibraryAsset), new { assetId = asset.Id }, asset);
        }

        [HttpDelete("{assetId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteLibraryAsset(int assetId)
        {
            LmsResponseHandler<LibraryAssetForDetailedDto> result = await _libraryAssestService.DeleteAsset(assetId);

            return ResultCheck(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(LibraryAssetForDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            LmsResponseHandler<LibraryAssetForDetailedDto> result = await _libraryAssestService.EditAsset(libraryAssetForUpdate);

            return ResultCheck(result);
        }

        [HttpGet("{assetId}", Name = nameof(GetLibraryAsset))]
        [ProducesResponseType(typeof(LibraryAssetForDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLibraryAsset(int assetId)
        {
            LmsResponseHandler<LibraryAssetForDetailedDto> result = await _libraryAssestService.GetAssetWithDetails(assetId);

            return ResultCheck(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(LibraryAssetForListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLibraryAssets([FromQuery] PaginationParams paginationParams)
        {
            PagedList<LibraryAssetForListDto> assets = await _libraryAssestService.GetPaginatedAssets(paginationParams);

            return ReturnPagination(assets);
        }

        [HttpGet("author/{authorId}")]
        [ProducesResponseType(typeof(LibraryAssetForDetailedDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAssetForAuthor(int authorId, [FromQuery] PaginationParams paginationParams)
        {
            PagedList<LibraryAssetForListDto> assets = await _libraryAssestService.GetAssetsByAuthor(paginationParams, authorId);

            return ReturnPagination(assets);
        }
    }
}
