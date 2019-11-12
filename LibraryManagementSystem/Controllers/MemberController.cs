using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Policy = PolicyRole.RequireLibrarianRole)]
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
        public async Task<ActionResult> GetMembers()
        {
            var users = await _memberService.SearchMembers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetpaginatedMembers([FromQuery]PaginationParams paginationParams)
        {
            var members = await _memberService.GetAllMembers(paginationParams);

            var membersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(members);

            Response.AddPagination(members.CurrentPage, members.PageSize,
                 members.TotalCount, members.TotalPages);

            return Ok(membersToReturn);
        }

        [HttpGet("advancedSearch/")]
        public async Task<IActionResult> AdvancedmemberSearch([FromQuery]UserForDetailedDto member)
        {
            var members = await _memberService.AdvancedMemberSearch(member);

            var membersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(members);

            return Ok(membersToReturn);
        }

        [HttpGet("{id}", Name = nameof(GetMember))]
        public async Task<IActionResult> GetMember(int id)
        {
            var user = await _memberService.GetMember(id);

            if (user == null)
            {
                return NotFound();
            }

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetMemberByCardNumber(int cardId)
        {
            var user = await _memberService.GetMemberByCardNumber(cardId);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMember(UserForUpdateDto userForUpdateDto)
        {
            var user = await _memberService.GetMember(userForUpdateDto.Id);

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

            member.UserName = member.Email;

            var result = await _memberService.CreateMember(member);

            if (result.Succeeded)
            {
                member = await _memberService.CompleteAddMember(member);

                var MemberToReturn = _mapper.Map<UserForDetailedDto>(member);

                MemberToReturn.LibraryCardNumber = member.LibraryCard.Id;

                return CreatedAtAction(nameof(GetMember), new { id = MemberToReturn.Id }, MemberToReturn);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _memberService.GetMember(id);

            if (user == null)
            {
                return NotFound();
            }

            await _memberService.DeleteMember(user);

            return NoContent();
        }
    }
}