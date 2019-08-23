using AutoMapper;
using LibraryManagementSystem.Helpers;
using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LibraryManagementSystem.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IEmailSender _emailSender;

        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, ILogger<AuthController> logger,
            IMapper mapper,
            UserManager<User> userManager, IOptions<AppSettings> appSettings,
            SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        //{
        //    const string role = "Member";

        //    userForRegisterDto.UserName = userForRegisterDto.Email;

        //    var userToCreate = _mapper.Map<User>(userForRegisterDto);

        //    var libraryCardDto = new LibraryCardForCreationDto();

        //    var newIdCard = _mapper.Map<LibraryCard>(libraryCardDto);

        //    var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

        //    var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

        //    newIdCard.UserId = userToReturn.Id;

        //    if (result.Succeeded)
        //    {
        //        await _userManager.AddToRoleAsync(userToCreate, role);

        //        _libraryRepo.Add(newIdCard);

        //        return CreatedAtRoute("GetUser",
        //            new { Controller = "Users", id = userToCreate.Id }, userToReturn);
        //    }

        //    return BadRequest(result.Errors);
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(userForLoginDto.Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

                if (result.Succeeded)
                {
                    var appUser = await _userManager.Users.Include(p => p.ProfilePicture)
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == userForLoginDto.Email.ToUpper());

                    var userToReturn = _mapper.Map<UserForDetailedDto>(appUser);

                    var roles = await _userManager.GetRolesAsync(user);

                    foreach (var role in roles)
                    {
                        userToReturn.Role = role;
                    }

                    _logger.LogInformation("Successful Login by Id: {0}, Email: {1}", user.Id, user.Email);

                    return Ok(new
                    {
                        token = GenerateJwtToken(appUser).Result,
                        user = userToReturn
                    });
                }
            }

            _logger.LogWarning("Unsuccessful login by user: {0}", userForLoginDto.Email);
            return BadRequest("Email or Password does not match");
        }

        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ResetPassword resetPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email);
                if (user == null || (await _userManager.IsInRoleAsync(user, nameof(EnumRoles.Member))))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Ok();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var encodedToken = HttpUtility.UrlEncode(code);

                var callbackUrl = new Uri(Request.Scheme + "://" + Request.Host + "/resetpassword/" + user.Id + "/" + encodedToken);

                var body = $"Hello {user.FirstName.ToLower()}, Please reset your password by clicking <a href='{callbackUrl}'>here</a>:";

                await _emailSender.SendEmail(resetPassword.Email, "Reset Password", body);

                return Ok();
            }

            return BadRequest("Something happened Please Try again later");
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByIdAsync(resetPassword.UserId.ToString());

            if (user == null || (await _userManager.IsInRoleAsync(user, nameof(EnumRoles.Member))))
            {
                return BadRequest("User does not exist");
            }

            //var code = GetResetCode(resetPassword.Token, resetCode);

            var decodedToken = HttpUtility.UrlDecode(resetPassword.Code);

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Code, resetPassword.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Token));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateJwtToken(User user, string code)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("ResetCode", code)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Token));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(60),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GetResetCode(string token, string key)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(token);

            var payload = jwtToken.Payload;

            payload.TryGetValue(key, out object value);

            return value.ToString();
        }
    }
}