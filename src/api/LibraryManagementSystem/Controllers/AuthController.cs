using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            LmsResponseHandler<LoginUserDto> result = await _authService.Login(userForLoginDto);

            return result.Succeeded ? Ok(result.Item) : BadRequest(result.Error);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest)
        {
            // TODO Fix Dto's
            AppUser user = await _authService.FindUserByEmail(forgotPasswordRequest.Email);

            if (!await _authService.IsResetEligible(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return NoContent();
            }

            await _authService.ForgotPassword(user, Request.Scheme, Request.Host);

            // TODO confirm if this should be NoContent
            return NoContent();
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            AppUser user = await _authService.FindUserById(resetPassword.UserId);

            if (!await _authService.IsResetEligible(user))
            {
                return BadRequest("User does not exist");
            }

            Microsoft.AspNetCore.Identity.IdentityResult result = await _authService.ResetPassword(user, resetPassword.Password, resetPassword.Code);

            return result.Succeeded ? NoContent() : BadRequest(result.Errors);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(TokenRequestDto tokenRequest)
        {
            LmsResponseHandler<TokenResponseDto> result = await _authService.RefreshToken(tokenRequest);

            return result.Succeeded ? Ok(result.Item) : Unauthorized();
        }

        [HttpPost("revoke")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RevokeToken(TokenRequestDto tokenRequest)
        {
            await _authService.RevokeToken(tokenRequest);

            return NoContent();
        }
    }
}
