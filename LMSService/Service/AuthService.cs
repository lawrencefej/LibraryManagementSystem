using LMSRepository.Dto;
using LMSRepository.Helpers;
using LMSRepository.Models;
using LMSService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LMSService.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<UserForDetailedDto> AddRoleToUser(UserForDetailedDto userToReturn, User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                userToReturn.Role = role;
            }

            return userToReturn;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task ForgotPassword(User user, string scheme, HostString host)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = HttpUtility.UrlEncode(code);

            var callbackUrl = new Uri(scheme + "://" + host + "/resetpassword/" + user.Id + "/" + encodedToken);

            var body = $"Hello {user.FirstName.ToLower()}, Please reset your password by clicking <a href='{callbackUrl}'>here</a>:";

            await _emailSender.SendEmail(user.Email, "Reset Password", body);
        }

        public async Task<IdentityResult> ResetPassword(User user, string password, string code)
        {
            var result = await _userManager.ResetPasswordAsync(user, code, password);

            return result;
        }

        public async Task<SignInResult> SignInUser(User user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            return result;
        }

        public async Task<User> GetUser(string email)
        {
            var user = await _userManager.Users.Include(p => p.ProfilePicture)
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());

            return user;
        }

        public async Task<string> GenerateJwtToken(User user, string jwtToken)
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

        public async Task<User> FindUserById(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user;
        }

        public async Task<bool> IsResetEligible(User user)
        {
            if (user == null || (await _userManager.IsInRoleAsync(user, nameof(EnumRoles.Member))))
            {
                return false;
            }

            return true;
        }
    }
}