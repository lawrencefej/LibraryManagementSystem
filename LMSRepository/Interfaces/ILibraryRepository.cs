using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface ILibraryRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<IList<User>> GetMembers();

        Task<IList<User>> GetAdmins();

        Task<IList<User>> GetLibrarians();

        Task<User> GetUser(int id);

        Task<List<Checkout>> GetCheckoutsForAsset(int id);

        Task<List<ReserveAsset>> GetReservesForAsset(int id);

        Task<List<Checkout>> GetCheckouts();

        Task<List<ReserveAsset>> GetReserves();

        Task<List<Checkout>> GetCheckoutsHistory();

        Task<List<ReserveAsset>> GetReservesHistory();

        Task<Checkout> GetCheckout(int id);

        Task<ReserveAsset> GetReserve(int id);

        Task<Status> GetStatus(string status);
    }
}