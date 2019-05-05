using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
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

        // GET: api/Photo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos()
        {
            return await _context.Photos.ToListAsync();
        }

        // GET: api/Photo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Photo>> GetPhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo == null)
            {
                return NotFound();
            }

            return photo;
        }

        // PUT: api/Photo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoto(int id, Photo photo)
        {
            if (id != photo.Id)
            {
                return BadRequest();
            }

            _context.Entry(photo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Photo
        //[HttpPost]
        //public async Task<ActionResult<Photo>> PostPhoto(Photo photo)
        //{
        //    _context.Photos.Add(photo);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
        //}

        [HttpPost("userphoto")]
        public async Task<IActionResult> AddPhotoForUser([FromForm]UserPhotoDto userPhotoDto)
        {
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

        // DELETE: api/Photo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Photo>> DeletePhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return photo;
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }
    }
}