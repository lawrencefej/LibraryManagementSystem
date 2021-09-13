using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Helpers;
using LMSEntities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LMSContracts.Interfaces
{
    public interface IAuthService
    {
        Task<LmsResponseHandler<LoginUserDto>> Login(UserForLoginDto userForLoginDto);
        Task<LmsResponseHandler<TokenResponseDto>> RefreshToken(TokenRequestDto tokenRequestDto);
        Task<AppUser> FindUserByEmail(string email);

        Task<AppUser> FindUserById(int userId);

        Task ForgotPassword(AppUser user, string scheme, HostString host);

        Task<IdentityResult> ResetPassword(AppUser user, string password, string code);

        Task<bool> IsResetEligible(AppUser user);

        Task RevokeToken(TokenRequestDto tokenRequestDto);
    }
}
