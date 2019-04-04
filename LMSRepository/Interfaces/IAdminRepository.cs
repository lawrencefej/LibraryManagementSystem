using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<User>> GetUsers();

        Task<User> GetAdminUser(int userId);

        Task CreateUser(User user, string password, string role);

        Task UpdateUser(User user, string newRole);
    }
}