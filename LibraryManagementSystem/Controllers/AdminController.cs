using LMSRepository.Data;
using LMSRepository.Dto;
using LMSRepository.Interfaces;
using LMSRepository.Models;
using LMSService.Interfacees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Controllers
{
    [Authorize(Policy = Helpers.Role.RequireAdminRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepo;
        private readonly IAdminService _adminService;

        public AdminController(
            DataContext context,
            UserManager<User> userManager,
            IUserRepository userRepo, IAdminService adminService)
        {
            _context = context;
            _userManager = userManager;
            _userRepo = userRepo;
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _adminService.GetAdminUsers();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AddAdminDto addAdminDto)
        {
            addAdminDto.CallbackUrl = (Request.Scheme + "://" + Request.Host + "/resetpassword/");

            var user = await _adminService.CreateUser(addAdminDto);

            return CreatedAtRoute("Get", new { id = user.Id }, user);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _adminService.GetAdminUser(id);

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateAdminDto updateAdminDto)
        {
            var user = await _adminService.GetAdminUser(updateAdminDto.Id);

            if (user == null)
            {
                return BadRequest("User for not found");
            }

            await _adminService.UpdateUser(updateAdminDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _adminService.DeleteUser(id);

            return NoContent();
        }
    }
}