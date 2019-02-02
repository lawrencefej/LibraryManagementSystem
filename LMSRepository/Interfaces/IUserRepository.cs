using LMSLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSLibrary.DataAccess
{
    public interface IUserRepository
    {
        Task<bool> SaveAll();

        Task<User> GetUser(int id);

        Task<IEnumerable<User>> GetUsers();

        Task<IEnumerable<Checkout>> GetUserCheckoutHistory(int memberId);

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByCardId(int cardId);

        Task<IEnumerable<ReserveAsset>> GetUserReservedAssets(int memberId);

        Task<IEnumerable<Checkout>> GetUserCurrentCheckouts(int id);

        Task<IEnumerable<ReserveAsset>> GetUserCurrentReservedAssets(int id);

        Task<LibraryCard> GetUserLibraryCard(int id);
    }
}