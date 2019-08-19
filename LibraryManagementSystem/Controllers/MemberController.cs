using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LMSRepository.Models;
using LMSService.Interfaces;
using LMSRepository.Dto;
using AutoMapper;
using LMSRepository.Helpers;
using LibraryManagementSystem.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.OData;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Policy = API.Helpers.Role.RequireLibrarianRole)]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;

        public MemberController(IMemberService memberService, IMapper mapper)
        {
            _memberService = memberService;
            _mapper = mapper;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _memberService.SearchMembers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetMembers([FromQuery]PaginationParams paginationParams)
        {
            var members = await _memberService.GetAllMembers(paginationParams);

            var membersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(members);

            Response.AddPagination(members.CurrentPage, members.PageSize,
                 members.TotalCount, members.TotalPages);

            return Ok(membersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _memberService.GetMembers(id);

            if (user == null)
            {
                return NotFound();
            }

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetUserByCardNumber(int cardId)
        {
            var user = await _memberService.GetMemberByCardNumber(cardId);

            return Ok(user);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(UserForUpdateDto userForUpdateDto)
        {
            var user = await _memberService.GetMembers(userForUpdateDto.Id);

            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userForUpdateDto, user);

            await _memberService.UpdateMember(user);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(MemberForCreation memberForCreation)
        {
            var member = _mapper.Map<User>(memberForCreation);

            member = await _memberService.AddMember(member);

            var MemberToReturn = _mapper.Map<UserForDetailedDto>(member);

            MemberToReturn.LibraryCardNumber = member.LibraryCard.Id;

            return CreatedAtAction("GetUser", new { id = MemberToReturn.Id }, MemberToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _memberService.GetMembers(id);

            if (user == null)
            {
                return NotFound();
            }

            await _memberService.DeleteMember(user);

            return NoContent();
        }
    }
}
