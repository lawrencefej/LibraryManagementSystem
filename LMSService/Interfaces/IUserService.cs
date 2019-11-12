using LMSRepository.Dto;
using LMSRepository.Models;
using System.Threading.Tasks;

namespace LMSService.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(int userID);

        Task UpdateUser(User user);

        void AddRoleToUserDto(UserForDetailedDto user);
    }
}