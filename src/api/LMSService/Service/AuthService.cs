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
            IList<string> roles = await _userManager.GetRolesAsync(user);

            foreach (string role in roles)
            {
                userToReturn.Role = role;
            }

            return userToReturn;
        }

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

            await _emailSender.SendEmail(user.Email, "Reset Password", body);
        }

        public async Task<IdentityResult> ResetPassword(AppUser user, string password, string code)
        {
            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, password);

            return result;
        }

        public async Task<SignInResult> SignInUser(AppUser user, string password)
        {
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            return result;
        }

        public async Task<AppUser> GetUser(string email)
        {
            AppUser user = await _userManager.Users.Include(p => p.ProfilePicture)
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());

            return user;
        }

        public async Task<string> GenerateJwtToken(AppUser user, string jwtToken)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            IList<string> roles = await _userManager.GetRolesAsync(user);

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtToken));

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
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
    }
}
