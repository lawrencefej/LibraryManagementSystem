using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            IEnumerable<AppUser> users = await _adminService.GetAdminUsers();

            IEnumerable<UserForDetailedDto> usersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);

            usersToReturn = _adminService.AddRoleToUsers(usersToReturn);

            return Ok(usersToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AddAdminDto addAdminDto)
        {
            addAdminDto.CallbackUrl = (Request.Scheme + "://" + Request.Host + "/resetpassword/");

            AppUser user = _mapper.Map<AppUser>(addAdminDto);

            user.UserName = user.Email;

            IdentityResult result = await _adminService.CreateUser(user, "password");

            if (result.Succeeded)
            {
                user = await _adminService.CompleteUserCreation(user, addAdminDto.Role, addAdminDto.CallbackUrl);

                UserForDetailedDto userToReturn = _mapper.Map<UserForDetailedDto>(user);

                userToReturn = _adminService.AddRoleToUser(userToReturn);

                return CreatedAtRoute("Get", new { id = userToReturn.Id }, userToReturn);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            AppUser user = await _adminService.GetAdminUser(id);

            UserForDetailedDto userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateAdminDto updateAdminDto)
        {
            AppUser user = await _adminService.GetAdminUser(updateAdminDto.Id);

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
            AppUser user = await _adminService.GetAdminUser(id);

            if (user == null)
            {
                NoContent();
            }

            await _adminService.DeleteUser(user);

            return NoContent();
        }
    }
}
