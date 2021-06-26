using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSRepository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IPhotoService _photoService;

        public PhotoController(DataContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        [HttpPost("userphoto")]
        public async Task<IActionResult> AddPhotoForUser([FromForm] UserPhotoDto userPhotoDto)
        {
            ResponseHandler result = await _photoService.AddPhotoForUser(userPhotoDto);

            if (result.IsSuccessful)
            {
                return Ok(result.Result);
            }

            //return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
            return BadRequest(result.Message);
        }

        [HttpPost("assetphoto")]
        public async Task<IActionResult> AddPhotoForAsset([FromForm] AssetPhotoDto assetPhotoDto)
        {
            ResponseHandler result = await _photoService.AddPhotoForAsset(assetPhotoDto);

            if (result.IsSuccessful)
            {
                return Ok(result.Result);
            }

            //return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
            return BadRequest(result.Message);
        }

        [HttpPost("librarycardphoto")]
        public async Task<IActionResult> AddPhotoForLibraryCard([FromForm] AssetPhotoDto assetPhotoDto)
        {
            ResponseHandler result = await _photoService.AddPhotoForAsset(assetPhotoDto);

            if (result.IsSuccessful)
            {
                return Ok(result.Result);
            }

            //return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
            return BadRequest(result.Message);
        }
    }
}
