using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSLibrary.DataAccess;
using LMSLibrary.Dto;
using LMSRepository.Dto;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/{userId}/[controller]")]
    [Authorize(Policy = "RequireLibrarianRole")]
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

        [HttpGet]
        public async Task<ActionResult> GetUsers(int userId)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var users = await _userService.GetUsers();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult> GetUser(int userId, int id)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUser(id);

            return Ok(user);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(int userId, string email)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByEmail(email);

            return Ok(user);
        }

        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetUserByCarNumber(int userId, int cardId)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByCardNumber(cardId);

            return Ok(user);
        }

        [HttpGet("search/")]
        public async Task<IActionResult> SearchUser(int userId, SearchUserDto searchUser)
        {
            if (!IsCurrentuser(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.SearchUser(searchUser);

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            //TODO
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var UserFromRepo = await _userRepo.GetUser(id);

            _mapper.Map(userForUpdateDto, UserFromRepo);

            if (await _userRepo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating user {id} failed on save");
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

        private bool IsCurrentuser(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return false;
            }

            return true;
        }
    }
}