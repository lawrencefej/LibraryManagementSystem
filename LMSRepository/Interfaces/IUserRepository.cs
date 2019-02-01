using LMSLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSLibrary.DataAccess
{
    public interface IUserRepository
    {
        Task<bool> SaveAll();

        Task<User> GetUser(int id);

        Task<List<User>> GetUsers();

        Task<List<Checkout>> GetUserCheckoutHistory(int memberId);

        Task<List<ReserveAsset>> GetUserReservedAssets(int memberId);

        Task<List<Checkout>> GetUserCurrentCheckouts(int id);

        Task<List<ReserveAsset>> GetUserCurrentReservedAssets(int id);

        Task<LibraryCard> GetUserLibraryCard(int id);
    }
}