using LMSRepository.Dto;
using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetAdminUsers();

        //Task<UserForDetailedDto> CreateUser(AddAdminDto addAdminDto);

        Task<User> CreateUser(User newUser, string newRole, string callbackUrl);

        IEnumerable<UserForDetailedDto> AddRoleToUsers(IEnumerable<UserForDetailedDto> users);

        UserForDetailedDto AddRoleToUser(UserForDetailedDto user);

        Task UpdateUser(User userforUpdate, string role);

        Task DeleteUser(User user);

        Task<User> GetAdminUser(int userId);
    }
}