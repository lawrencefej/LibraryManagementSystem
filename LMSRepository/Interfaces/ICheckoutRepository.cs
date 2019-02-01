using LMSLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface ICheckoutRepository
    {
        Task<List<Checkout>> GetAllCheckouts();

        Task<Checkout> GetCheckout(int id);

        Task<List<Checkout>> GetCheckoutHistory(int cardId);

        Task<Checkout> GetLatestCheckout(int id);

        Task<int> GetMemberCurrentCheckoutAmount(int cardId);

        Task<IEnumerable<Checkout>> GetCheckoutsForMember(int id);
    }
}