using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LMSContracts.Interfaces;
using LMSEntities.Configuration;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using LMSRepository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LMSService.Service
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IOptions<JwtSettings> jwtSettings, DataContext dataContext, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _context = dataContext;
        }

        public async Task<TokenResponseDto> GetLoginToken(AppUser user, string ipAddress)
        {
            string token = GenerateJwtToken(user);
            return new TokenResponseDto()
            {
                Token = token,
                RefreshToken = await GenerateRefreshToken(user.Id, token, ipAddress)
            };
        }

        public async Task<LmsResponseHandler<TokenResponseDto>> RefreshToken(TokenRequestDto tokenRequestDto, string ipAddress)
        {
            LmsResponseHandler<ClaimsPrincipal> result = await ValidateToken(tokenRequestDto, ipAddress);

            if (result.Succeeded)
            {
                AppUser user = await GetUserDetails(result.Item.Claims.ElementAt(1).Value);
                string jwtToken = GenerateJwtToken(user);

                return LmsResponseHandler<TokenResponseDto>.Successful(new TokenResponseDto()
                {
                    Token = jwtToken,
                    RefreshToken = await GenerateRefreshToken(user.Id, jwtToken, ipAddress)
                });
            }

            return LmsResponseHandler<TokenResponseDto>.Failed("");
        }

        public async Task RevokeToken(string token)
        {
            RefreshToken storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);

            if (storedRefreshToken != null)
            {
                await RemoveToken(storedRefreshToken);
            }
        }

        private async Task<LmsResponseHandler<ClaimsPrincipal>> ValidateToken(TokenRequestDto tokenRequest, string requestIpAddress)
        {
            LmsResponseHandler<ClaimsPrincipal> result = GetPrincipalFromToken(tokenRequest.Token);

            if (!result.Succeeded)
            {
                return LmsResponseHandler<ClaimsPrincipal>.Failed("");
            }

            RefreshToken storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedRefreshToken == null)
            {
                return LmsResponseHandler<ClaimsPrincipal>.Failed("");
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate || storedRefreshToken.Invalidated || storedRefreshToken.Used || storedRefreshToken.JwtId != tokenRequest.Token || storedRefreshToken.RequestIp != requestIpAddress)
            {
                await RemoveToken(storedRefreshToken);
                return LmsResponseHandler<ClaimsPrincipal>.Failed("");
            }

            await RemoveToken(storedRefreshToken);

            return LmsResponseHandler<ClaimsPrincipal>.Successful(result.Item);
        }

        private async Task RemoveToken(RefreshToken refreshToken)
        {
            _context.Remove(refreshToken);
            await _context.SaveChangesAsync();
        }

        private async Task<string> GenerateRefreshToken(int userId, string token, string ipAddress)
        {
            RefreshToken refreshToken = new()
            {
                JwtId = token,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetime),
                Token = RandomString(35) + Guid.NewGuid(),
                UserId = userId,
                RequestIp = ipAddress
            };

            _context.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        private string GenerateJwtToken(AppUser user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRoles.FirstOrDefault().Role.Name)
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenLifetime),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private LmsResponseHandler<ClaimsPrincipal> GetPrincipalFromToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = false, //here we are saying that we don't care about the token's expiration date
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new();

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                return !IsJwtWithValidSecurityAlgorithm(validatedToken)
                    ? LmsResponseHandler<ClaimsPrincipal>.Failed("")
                    : LmsResponseHandler<ClaimsPrincipal>.Successful(principal);
            }
            catch
            {
                return LmsResponseHandler<ClaimsPrincipal>.Failed("");
            }

        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                bool result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                if (result)
                {
                    return true;
                }
            }

            return false;
        }



        private static string RandomString(int length)
        {
            byte[] randomNumber = new byte[length];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<AppUser> GetUserDetails(string email)
        {
            return await _userManager.Users.Include(p => p.ProfilePicture)
                .Include(p => p.UserRoles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());
        }
    }
}
