using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LMSLibrary.DataAccess;
using LMSLibrary.Models;
using System.Security.Claims;
using LMSLibrary.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.API.Helpers;

namespace LibraryManagement.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UsersController(DataContext context, IUserRepository userRepo, IMapper mapper)
        {
            _context = context;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        // GET: api/Users
        [Authorize(Policy = "RequireLibrarianRole")]
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _userRepo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [Authorize(Policy = "RequireMemberRole")]
        [HttpGet("{id}", Name ="GetUser")]
        public async Task<ActionResult> GetUser(int id)
        {
            if (!IsCurrentuser(id))
            {
                return Unauthorized();
            }

            var user = await _userRepo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
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

        // DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<User>> DeleteUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}

        [HttpGet("{id}/checkouthistory")]
        public async Task<IActionResult> GetUserCheckoutHistory(int id)
        {
            if (!IsCurrentuser(id) )
            {
                return Unauthorized();
            }

            var libraryCard = await  _userRepo.GetUserLibraryCard(id);

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

        //private async Task<bool> IsCurrentUserCard(int id, int cardId)
        //{
        //    var userCard = await _userRepo.GetUser(id);

        //    if (cardId != userCard.LibraryCard.Id)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
