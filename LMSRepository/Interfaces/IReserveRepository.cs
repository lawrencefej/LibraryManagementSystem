using LMSRepository.Interfaces.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface IReserveRepository
    {
        Task<IEnumerable<ReserveAsset>> GetAllReserves();

        Task<ReserveAsset> GetReserve(int id);

        Task<int> GetMemberCurrentReserveAmount(int cardId);

        Task<IEnumerable<ReserveAsset>> GetReservesForMember(int id);

        Task<IEnumerable<ReserveAsset>> GetExpiringReserves();
    }
}