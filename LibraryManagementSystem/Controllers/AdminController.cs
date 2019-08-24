using AutoMapper;
using LMSRepository.Dto;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagementSystem.API.Controllers
{
    [Authorize(Policy = Helpers.Role.RequireAdminRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public AdminController(IMapper mapper, IAdminService adminService)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _adminService.GetAdminUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);

            usersToReturn = _adminService.AddRoleToUsers(usersToReturn);

            return Ok(usersToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AddAdminDto addAdminDto)
        {
            addAdminDto.CallbackUrl = (Request.Scheme + "://" + Request.Host + "/resetpassword/");

            var user = _mapper.Map<User>(addAdminDto);

            user = await _adminService.CreateUser(user, addAdminDto.Role, addAdminDto.CallbackUrl);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            userToReturn = _adminService.AddRoleToUser(userToReturn);

            return CreatedAtRoute("Get", new { id = userToReturn.Id }, userToReturn);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _adminService.GetAdminUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateAdminDto updateAdminDto)
        {
            var user = await _adminService.GetAdminUser(updateAdminDto.Id);

            if (user == null)
            {
                return BadRequest("User for not found");
            }

            await _adminService.UpdateUser(user, updateAdminDto.Role);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _adminService.GetAdminUser(id);

            if (user == null)
            {
                NoContent();
            }

            await _adminService.DeleteUser(user);

            return NoContent();
        }
    }
}