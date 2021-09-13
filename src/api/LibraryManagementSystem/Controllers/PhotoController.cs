
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("userphoto/{userId}")]
        [ProducesResponseType(typeof(PhotoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPhotoForUser(int userId, IFormFile file)
        {
            LmsResponseHandler<PhotoResponseDto> result = await _photoService.AddPhotoForUser(file, userId);

            return result.Succeeded ? Ok(result.Item) : BadRequest(result.Error);
        }

        [HttpPost("libraryCard/{cardId}")]
        [ProducesResponseType(typeof(PhotoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPhotoForCard(int cardId, IFormFile file)
        {
            LmsResponseHandler<PhotoResponseDto> result = await _photoService.AddPhotoForCard(file, cardId);

            return result.Succeeded ? Ok(result.Item) : BadRequest(result.Error);
        }

        [HttpPost("assetphoto/{assetId}")]
        [ProducesResponseType(typeof(PhotoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPhotoForAsset(int assetId, IFormFile file)
        {
            LmsResponseHandler<PhotoResponseDto> result = await _photoService.AddPhotoForAsset(file, assetId);

            return result.Succeeded ? Ok(result.Item) : BadRequest(result.Error);
        }
    }
}
