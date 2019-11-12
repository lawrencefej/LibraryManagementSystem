using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using LibraryManagementSystem.API.Helpers;
using LMSService.Interfaces;
using AutoMapper;
using LMSRepository.Dto;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Policy = PolicyRole.RequireLibrarianRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (!IsLoggedInUser(id))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            _userService.AddRoleToUserDto(userToReturn);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserForUpdateDto userForUpdate)
        {
            if (!IsLoggedInUser(id))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUser(id);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (user.Email != userForUpdate.Email)
            {
                return Forbid();
            }
            _mapper.Map(userForUpdate, user);

            await _userService.UpdateUser(user);

            return NoContent();
        }

        private bool IsLoggedInUser(int id)
        {
            return id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}