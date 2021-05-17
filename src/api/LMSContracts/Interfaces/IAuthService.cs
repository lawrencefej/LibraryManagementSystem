using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LMSContracts.Interfaces
{
    public interface IAuthService
    {
        Task<AppUser> FindUserByEmail(string email);

        Task<AppUser> FindUserById(int userId);

        Task<SignInResult> SignInUser(AppUser user, string password);

        Task<UserForDetailedDto> AddRoleToUser(UserForDetailedDto userToReturn, AppUser user);

        Task<string> GenerateJwtToken(AppUser user, string token);

        Task ForgotPassword(AppUser user, string scheme, HostString host);

        Task<AppUser> GetUser(string email);

        Task<IdentityResult> ResetPassword(AppUser user, string password, string code);

        Task<bool> IsResetEligible(AppUser user);
    }
}
