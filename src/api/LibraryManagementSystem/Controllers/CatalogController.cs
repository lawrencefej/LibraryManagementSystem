using System.Threading.Tasks;
using LibraryManagementSystem.API.Helpers;
using LibraryManagementSystem.Controllers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = Role.RequireLibrarianRole)]
    [ApiController]
    public class CatalogController : BaseApiController<LibraryAssetForDetailedDto, LibraryAssetForListDto>
    {
        private readonly ILibraryAssetService _libraryAssestService;

        public CatalogController(ILibraryAssetService libraryAssestService)
        {
            _libraryAssestService = libraryAssestService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLibraryAsset(LibraryAssetForCreationDto libraryAssetForCreation)
        {
            LibraryAssetForDetailedDto asset = await _libraryAssestService.AddAsset(libraryAssetForCreation);

            return CreatedAtRoute(nameof(GetLibraryAsset), new { assetId = asset.Id }, asset);
        }

        [HttpDelete("{assetId}")]
        public async Task<IActionResult> DeleteLibraryAsset(int assetId)
        {
            LmsResponseHandler<LibraryAssetForDetailedDto> result = await _libraryAssestService.DeleteAsset(assetId);

            return ResultCheck(result);
        }

        [HttpPut]
        public async Task<IActionResult> EditAsset(LibraryAssetForUpdateDto libraryAssetForUpdate)
        {
            LmsResponseHandler<LibraryAssetForDetailedDto> result = await _libraryAssestService.EditAsset(libraryAssetForUpdate);

            return ResultCheck(result);
        }

        [HttpGet("{assetId}", Name = nameof(GetLibraryAsset))]
        public async Task<IActionResult> GetLibraryAsset(int assetId)
        {
            LmsResponseHandler<LibraryAssetForDetailedDto> result = await _libraryAssestService.GetAssetWithDetails(assetId);

            return ResultCheck(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLibraryAssets([FromQuery] PaginationParams paginationParams)
        {
            PagedList<LibraryAssetForListDto> assets = await _libraryAssestService.GetPaginatedAssets(paginationParams);

            return ReturnPagination(assets);
        }

        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetAssetForAuthor(int authorId, [FromQuery] PaginationParams paginationParams)
        {
            PagedList<LibraryAssetForListDto> assets = await _libraryAssestService.GetAssetsByAuthor(paginationParams, authorId);

            return ReturnPagination(assets);
        }
    }
}
