using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Interfaces;
using LMSRepository.Interfaces.Dto;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepo, IMapper mapper, IUserService userService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _userService = userService;
        }

        [Authorize(Policy = Role.RequireLibrarianRole)]
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult> GetUser(int id)
        {
            if (!IsCurrentuser(id))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUser(id);

            return Ok(user);
        }

        [Authorize(Policy = Role.RequireLibrarianRole)]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmail(email);

            return Ok(user);
        }

        [Authorize(Policy = Role.RequireLibrarianRole)]
        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetUserByCarNumber(int cardId)
        {
            var user = await _userService.GetUserByCardNumber(cardId);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (!IsCurrentuser(id))
            {
                return Unauthorized();
            }

            await _userService.UpdateUser(userForUpdateDto);

            return NoContent();
        }

        [HttpGet("{id}/checkouthistory")]
        public async Task<IActionResult> GetUserCheckoutHistory(int id)
        {
            if (!IsCurrentuser(id))
            {
                return Unauthorized();
            }

            var libraryCard = await _userRepo.GetUserLibraryCard(id);

            var checkouthistory = await _userRepo.GetUserCheckoutHistory(libraryCard.Id);

            var checkoutHistoryForReturn = _mapper.Map<IEnumerable<CheckoutForReturnDto>>(checkouthistory);

            return Ok(checkoutHistoryForReturn);
        }

        [HttpGet("{id}/reservehistory")]
        public async Task<IActionResult> GetUserReserveHistory(int id)
        {
            if (!IsCurrentuser(id))
            {
                return Unauthorized();
            }

            var libraryCard = await _userRepo.GetUserLibraryCard(id);

            var reserveHistory = await _userRepo.GetUserReservedAssets(libraryCard.Id);

            var reserveHistoryForReturn = _mapper.Map<IEnumerable<ReserveForReturnDto>>(reserveHistory);

            return Ok(reserveHistoryForReturn);
        }

        [HttpGet("{id}/reserves/")]
        public async Task<IActionResult> GetUserCurrentReserve(int id)
        {
            if (!IsCurrentuser(id))
            {
                return Unauthorized();
            }

            var libraryCard = await _userRepo.GetUserLibraryCard(id);

            var reserveHistory = await _userRepo.GetUserCurrentReservedAssets(libraryCard.Id);

            var reserveHistoryForReturn = _mapper.Map<IEnumerable<ReserveForReturnDto>>(reserveHistory);

            return Ok(reserveHistoryForReturn);
        }

        [Authorize(Policy = Role.RequireLibrarianRole)]
        [HttpGet("search/")]
        public async Task<IActionResult> SearchUsers([FromQuery]string searchString)
        {
            var users = await _userService.SearchUsers(searchString);

            return Ok(users);
        }

        [Authorize(Policy = Role.RequireLibrarianRole)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAuthor(int userId)
        {
            await _userService.DeleteUser(userId);

            return NoContent();
        }

        private bool IsCurrentuser(int id)
        {
            var currentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (id != currentUser && !(User.IsInRole(Role.Librarian) || User.IsInRole(Role.Admin)))
            {
                return false;
            }

            return true;
        }
    }
}