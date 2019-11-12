using LibraryManagementSystem.API.Helpers;
using LMSRepository.Dto;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = PolicyRole.RequireLibrarianRole)]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("userphoto")]
        public async Task<IActionResult> AddPhotoForMember([FromForm]UserPhotoDto userPhotoDto)
        {
            var result = await _photoService.AddPhotoForUser(userPhotoDto);

            if (result.IsSuccessful)
            {
                return Ok(result.Result);
            }

            //return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
            return BadRequest(result.Message);
        }

        [HttpPost("user-profile-picture")]
        public async Task<IActionResult> AddPhotoForUser([FromForm]UserPhotoDto userPhotoDto)
        {
            if (userPhotoDto.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var result = await _photoService.AddPhotoForUser(userPhotoDto);

            if (result.IsSuccessful)
            {
                return Ok(result.Result);
            }

            //return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
            return BadRequest(result.Message);
        }

        [HttpPost("assetphoto")]
        public async Task<IActionResult> AddPhotoForAsset([FromForm]AssetPhotoDto assetPhotoDto)
        {
            var result = await _photoService.AddPhotoForAsset(assetPhotoDto);

            if (result.IsSuccessful)
            {
                return Ok(result.Result);
            }

            //return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
            return BadRequest(result.Message);
        }
    }
}