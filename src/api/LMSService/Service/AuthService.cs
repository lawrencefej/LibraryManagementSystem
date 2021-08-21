using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Enumerations;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSService.Service
{
    public class AuthService : IAuthService
    {
        // private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager,
                           ILogger<AuthService> logger,
                           IMapper mapper, ITokenService tokenService,
                           IHttpContextAccessor context)
        {
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // public async Task<UserForDetailedDto> AddRoleToUser(UserForDetailedDto userToReturn, AppUser user)
        // {
        //     IList<string> roles = await _userManager.GetRolesAsync(user);

        //     foreach (string role in roles)
        //     {
        //         userToReturn.Role = role;
        //     }

        //     return userToReturn;
        // }

        public async Task<AppUser> FindUserByEmail(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task ForgotPassword(AppUser user, string scheme, HostString host)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);

            string encodedToken = HttpUtility.UrlEncode(code);

            Uri callbackUrl = new Uri(scheme + "://" + host + "/resetpassword/" + user.Id + "/" + encodedToken);

            string body = $"Hello {user.FirstName.ToLower()}, Please reset your password by clicking <a href='{callbackUrl}'>here</a>:";

            // TODO fix email
            // await _emailSender.SendEmail(user.Email, "Reset Password", body);
        }

        public async Task<IdentityResult> ResetPassword(AppUser user, string password, string code)
        {
            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, password);

            return result;
        }

        public async Task RevokeToken(TokenRequestDto tokenRequestDto)
        {
            await _tokenService.RevokeToken(tokenRequestDto.RefreshToken);
        }

        public async Task<AppUser> FindUserById(int userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId.ToString());

            return user;
        }

        public async Task<bool> IsResetEligible(AppUser user)
        {
            return user != null && !await _userManager.IsInRoleAsync(user, nameof(LmsAppRoles.Member));
        }

        public async Task<LmsResponseHandler<LoginUserDto>> Login(UserForLoginDto userForLoginDto)
        {
            AppUser user = await _userManager.FindByEmailAsync(userForLoginDto.Email);

            if (user != null)
            {
                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

                if (result.Succeeded)
                {
                    user = await GetUserDetails(user.Id);

                    LoginUserDto userToReturn = await GenerateResponse(user, GetIpAddress());

                    _logger.LogInformation("Successful Login by Id: {0}, Email: {1}", userToReturn.Id, userToReturn.Email);

                    return LmsResponseHandler<LoginUserDto>.Successful(userToReturn);
                }
            }

            _logger.LogWarning("Unsuccessful login by user: {0}", userForLoginDto.Email);

            return LmsResponseHandler<LoginUserDto>.Failed("Email or Password does not match");
        }

        public async Task<LmsResponseHandler<TokenResponseDto>> RefreshToken(TokenRequestDto tokenRequestDto)
        {
            LmsResponseHandler<TokenResponseDto> result = await _tokenService.RefreshToken(tokenRequestDto, GetIpAddress());

            return result.Succeeded
                ? LmsResponseHandler<TokenResponseDto>.Successful(result.Item)
                : LmsResponseHandler<TokenResponseDto>.Failed("");
        }

        private async Task<LoginUserDto> GenerateResponse(AppUser user, string clientIpAddress)
        {
            TokenResponseDto tokenResponse = await _tokenService.GetLoginToken(user, clientIpAddress);
            LoginUserDto userToReturn = _mapper.Map<LoginUserDto>(user);
            userToReturn.Token = tokenResponse.Token;
            userToReturn.RefreshToken = tokenResponse.RefreshToken;
            return userToReturn;
        }

        private async Task<AppUser> GetUserDetails(int userId)
        {
            return await _userManager.Users.Include(p => p.ProfilePicture)
                        .Include(p => p.UserRoles)
                        .ThenInclude(r => r.Role)
                        .FirstOrDefaultAsync(u => u.Id == userId);
        }



        private string GetIpAddress()
        {
            return _context.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
