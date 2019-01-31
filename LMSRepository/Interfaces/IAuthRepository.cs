using LMSLibrary.Models;
using System.Threading.Tasks;

namespace LMSLibrary.DataAccess
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);

        Task<User> Login(string emailAddress, string password);

        Task<bool> UserExists(string username);
    }
}
