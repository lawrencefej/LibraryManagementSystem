using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;
using Microsoft.AspNetCore.Identity;

namespace LMSContracts.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AppUser>> GetAdminUsers();

        Task<IdentityResult> CreateUser(AppUser user);
        Task<IdentityResult> CreateUser(AppUser user, string password);

        Task<AppUser> CompleteUserCreation(AppUser newUser, string newRole, string callbackUrl);

        IEnumerable<UserForDetailedDto> AddRoleToUsers(IEnumerable<UserForDetailedDto> users);

        UserForDetailedDto AddRoleToUser(UserForDetailedDto user);

        Task UpdateUser(AppUser userforUpdate, string role);

        Task DeleteUser(AppUser user);

        Task<AppUser> GetAdminUser(int userId);
    }
}
