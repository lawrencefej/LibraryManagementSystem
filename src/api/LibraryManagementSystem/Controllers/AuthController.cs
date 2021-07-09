using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementSystem.Helpers;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;
using LMSService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LibraryManagementSystem.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _authService.FindUserByEmail(userForLoginDto.Email);

            if (user != null)
            {
                var result = await _authService.SignInUser(user, userForLoginDto.Password);

                if (result.Succeeded)
                {
                    var appUser = await _authService.GetUser(userForLoginDto.Email);

                    var userToReturn = _mapper.Map<UserForDetailedDto>(appUser);

                    userToReturn = await _authService.AddRoleToUser(userToReturn, appUser);

                    _logger.LogInformation("Successful Login by Id: {0}, Email: {1}", user.Id, user.Email);

                    return Ok(new
                    {
                        // token = await _authService.GenerateJwtToken(appUser, _appSettings.Token),
                        token = LmsTokens.GenerateJwtToken(appUser, _appSettings.Token),
                        user = userToReturn
                    });
                }
            }

            _logger.LogWarning("Unsuccessful login by user: {0}", userForLoginDto.Email);
            return BadRequest("Email or Password does not match");
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ResetPassword resetPassword)
        {
            AppUser user = await _authService.FindUserByEmail(resetPassword.Email);

            if (!await _authService.IsResetEligible(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok();
            }

            await _authService.ForgotPassword(user, Request.Scheme, Request.Host);

            // TODO confirm if this should be NoContent
            return Ok();
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            AppUser user = await _authService.FindUserById(resetPassword.UserId);

            if (!await _authService.IsResetEligible(user))
            {
                return BadRequest("User does not exist");
            }

            Microsoft.AspNetCore.Identity.IdentityResult result = await _authService.ResetPassword(user, resetPassword.Password, resetPassword.Code);

            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
    }
}
