using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LibraryManagementSystem.Controllers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = Role.RequireLibrarianRole)]
    [ApiController]
    public class CatalogController : BaseApiController<LibraryAssetForDetailedDto>
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
            LibraryAssetForDetailedDto asset = await _libraryAssestService.AddAsset(libraryAssetForCreation);

            return CreatedAtRoute(nameof(GetLibraryAsset), new { assetId = asset.Id }, asset);
        }

        [HttpDelete("{assetId}")]
        public async Task<IActionResult> DeleteLibraryAsset(LibraryAssetForDetailedDto assetFordel)
        {
            LmsResponseHandler<LibraryAssetForDetailedDto> result = await _libraryAssestService.DeleteAsset(assetFordel);

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

        private IActionResult ReturnPagination(PagedList<LibraryAssetForListDto> assets)
        {
            Response.AddPagination(assets.CurrentPage, assets.PageSize,
                             assets.TotalCount, assets.TotalPages);

            return Ok(assets);
        }
    }
}
