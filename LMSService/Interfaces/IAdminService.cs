using LMSRepository.Dto;
using LMSRepository.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetAdminUsers();

        Task<IdentityResult> CreateUser(User user);

        Task<User> CompleteUserCreation(User newUser, string newRole, string callbackUrl);

        IEnumerable<UserForDetailedDto> AddRoleToUsers(IEnumerable<UserForDetailedDto> users);

        UserForDetailedDto AddRoleToUser(UserForDetailedDto user);

        Task UpdateUser(User userforUpdate, string role);

        Task DeleteUser(User user);

        Task<User> GetAdminUser(int userId);
    }
}