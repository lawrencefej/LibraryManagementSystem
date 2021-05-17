using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LMSContracts.Interfaces;
using LMSEntities.DataTransferObjects;
using LMSEntities.Enumerations;
using LMSEntities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LMSService.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<UserForDetailedDto> AddRoleToUser(UserForDetailedDto userToReturn, AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                userToReturn.Role = role;
            }

            return userToReturn;
        }

        public async Task<AppUser> FindUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task ForgotPassword(AppUser user, string scheme, HostString host)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = HttpUtility.UrlEncode(code);

            var callbackUrl = new Uri(scheme + "://" + host + "/resetpassword/" + user.Id + "/" + encodedToken);

            var body = $"Hello {user.FirstName.ToLower()}, Please reset your password by clicking <a href='{callbackUrl}'>here</a>:";

            await _emailSender.SendEmail(user.Email, "Reset Password", body);
        }

        public async Task<IdentityResult> ResetPassword(AppUser user, string password, string code)
        {
            var result = await _userManager.ResetPasswordAsync(user, code, password);

            return result;
        }

        public async Task<SignInResult> SignInUser(AppUser user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            return result;
        }

        public async Task<AppUser> GetUser(string email)
        {
            var user = await _userManager.Users.Include(p => p.ProfilePicture)
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());

            return user;
        }

        public async Task<string> GenerateJwtToken(AppUser user, string jwtToken)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken));

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

        public async Task<AppUser> FindUserById(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user;
        }

        public async Task<bool> IsResetEligible(AppUser user)
        {
            return user != null && !await _userManager.IsInRoleAsync(user, nameof(RolesEnum.Member));
        }
    }
}
