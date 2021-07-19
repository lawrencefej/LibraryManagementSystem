using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Extensions;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> GetMembers()
        {
            IEnumerable<AppUser> users = await _memberService.SearchMembers();

            IEnumerable<UserForListDto> usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("pagination/")]
        public async Task<IActionResult> GetpaginatedMembers([FromQuery] PaginationParams paginationParams)
        {
            PagedList<AppUser> members = await _memberService.GetAllMembers(paginationParams);

            IEnumerable<UserForDetailedDto> membersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(members);

            Response.AddPagination(members.CurrentPage, members.PageSize,
                 members.TotalCount, members.TotalPages);

            return Ok(membersToReturn);
        }

        [HttpGet("advancedsearch/")]
        public async Task<IActionResult> AdvancedmemberSearch([FromQuery] UserForDetailedDto member)
        {
            IEnumerable<AppUser> members = await _memberService.AdvancedMemberSearch(member);

            IEnumerable<UserForDetailedDto> membersToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(members);

            return Ok(membersToReturn);
        }

        [HttpGet("{id}", Name = nameof(GetMember))]
        public async Task<IActionResult> GetMember(int id)
        {
            AppUser user = await _memberService.GetMember(id);

            if (user == null)
            {
                return NotFound();
            }

            UserForDetailedDto userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetMemberByCardNumber(int cardId)
        {
            AppUser user = await _memberService.GetMemberByCardNumber(cardId);

            if (user == null)
            {
                return NotFound();
            }

            UserForDetailedDto userToReturn = _mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMember(UserForUpdateDto userForUpdateDto)
        {
            AppUser user = await _memberService.GetMember(userForUpdateDto.Id);

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
            AppUser member = _mapper.Map<AppUser>(memberForCreation);

            member.UserName = member.Email;

            IdentityResult result = await _memberService.CreateMember(member);

            if (result.Succeeded)
            {
                member = await _memberService.CompleteAddMember(member);

                UserForDetailedDto MemberToReturn = _mapper.Map<UserForDetailedDto>(member);

                MemberToReturn.LibraryCardNumber = member.LibraryCard.Id;

                return CreatedAtAction(nameof(GetMember), new { id = MemberToReturn.Id }, MemberToReturn);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            AppUser user = await _memberService.GetMember(id);

            if (user == null)
            {
                return NotFound();
            }

            await _memberService.DeleteMember(user);

            return NoContent();
        }
    }
}
