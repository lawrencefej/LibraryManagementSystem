using LMSRepository.Interfaces.Models;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);

        Task<User> Login(string emailAddress, string password);

        Task<bool> UserExists(string username);
    }
}