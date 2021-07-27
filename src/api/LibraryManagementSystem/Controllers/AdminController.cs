using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.API.Helpers;
using LibraryManagementSystem.Controllers;
using LibraryManagementSystem.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [Authorize(Policy = Role.RequireAdminRole)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseApiController<AdminUserForListDto, AdminUserForListDto>
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public AdminController(IMapper mapper, IAdminService adminService)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdminUserForListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdminUsers([FromQuery] PaginationParams paginationParams)
        {
            PagedList<AdminUserForListDto> users = await _adminService.GetAdminUsers(paginationParams);

            return ReturnPagination(users);
        }

        [HttpGet("{userId}", Name = nameof(GetAdmin))]
        [ProducesResponseType(typeof(AdminUserForListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdmin(int userId)
        {
            LmsResponseHandler<AdminUserForListDto> result = await _adminService.GetAdminUser(userId);

            return ResultCheck(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AdminUserForListDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(AddAdminDto addAdminDto)
        {
            LmsResponseHandler<AdminUserForListDto> result = await _adminService.CreateUser(addAdminDto);

            return result.Succeeded ? CreatedAtRoute(nameof(GetAdmin), new { userId = result.Item.Id }, result.Item) : BadRequest(result.Errors);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AdminUserForListDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(UpdateAdminRoleDto updateAdminDto)
        {
            LmsResponseHandler<AdminUserForListDto> result = await _adminService.UpdateUser(updateAdminDto);

            return ResultCheck(result);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(AdminUserForListDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int userId)
        {
            LmsResponseHandler<AdminUserForListDto> result = await _adminService.DeleteUser(userId);

            return ResultCheck(result);
        }

        [ServiceFilter(typeof(DevOnlyActionFilter))]
        [HttpPost]
        [Route("password")]
        public async Task<IActionResult> CreateAdminWithPassword(AddAdminDto addAdminDto)
        {
            LmsResponseHandler<AdminUserForListDto> result = await _adminService.CreateUser(addAdminDto, true);

            return result.Succeeded ? CreatedAtRoute(nameof(GetAdmin), new { id = result.Item.Id }, result.Item) : BadRequest(result.Errors);
        }
    }
}
