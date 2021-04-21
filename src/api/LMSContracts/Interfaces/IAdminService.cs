using System.Collections.Generic;
using System.Threading.Tasks;
using LMSEntities.DataTransferObjects;
using LMSEntities.Models;
using Microsoft.AspNetCore.Identity;

namespace LMSContracts.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetAdminUsers();

        Task<IdentityResult> CreateUser(User user);
        Task<IdentityResult> CreateUser(User user, string password);

        Task<User> CompleteUserCreation(User newUser, string newRole, string callbackUrl);

        IEnumerable<UserForDetailedDto> AddRoleToUsers(IEnumerable<UserForDetailedDto> users);

        UserForDetailedDto AddRoleToUser(UserForDetailedDto user);

        Task UpdateUser(User userforUpdate, string role);

        Task DeleteUser(User user);

        Task<User> GetAdminUser(int userId);
    }
}
