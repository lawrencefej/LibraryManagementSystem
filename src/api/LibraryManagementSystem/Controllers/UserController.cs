using System.Threading.Tasks;
using LibraryManagementSystem.API.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Policy = Role.RequireLibrarianRole)]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseApiController<UserForDetailedDto, UserForDetailedDto>
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}", Name = nameof(GetUser))]
        [ProducesResponseType(typeof(UserForDetailedDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser(int userId)
        {
            LmsResponseHandler<UserForDetailedDto> result = await _userService.GetUser(userId);

            return ResultCheck(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserProfile(UserForUpdateDto userForUpdate)
        {
            LmsResponseHandler<UserForDetailedDto> result = await _userService.UpdateUserProfile(userForUpdate);

            return ResultCheck(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckoutAsset(UserPasswordResetRequest userPasswordReset)
        {
            LmsResponseHandler<UserForDetailedDto> result = await _userService.ResetPassword(userPasswordReset);

            return ResultCheck(result);
        }
    }
}
