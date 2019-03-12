using LMSRepository.Dto;
using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> SaveAll();

        Task<User> GetUser(int id);

        Task<User> SearchUser(SearchUserDto searchUser);

        Task<IEnumerable<User>> SearchUsers(string searchString);

        Task<IEnumerable<User>> GetUsers();

        Task<IEnumerable<Checkout>> GetUserCheckoutHistory(int memberId);

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByCardId(int? cardId);

        Task<IEnumerable<ReserveAsset>> GetUserReservedAssets(int memberId);

        Task<IEnumerable<Checkout>> GetUserCurrentCheckouts(int id);

        Task<IEnumerable<ReserveAsset>> GetUserCurrentReservedAssets(int id);

        Task<LibraryCard> GetUserLibraryCard(int id);
    }
}