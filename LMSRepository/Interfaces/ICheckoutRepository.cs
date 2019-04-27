using LMSRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSRepository.Interfaces
{
    public interface ICheckoutRepository
    {
        Task<List<Checkout>> GetAllCheckouts();

        Task<Checkout> GetCheckout(int id);

        Task<List<Checkout>> GetCheckoutHistory(int cardId);

        Task<IEnumerable<Checkout>> GetCheckoutsForMember(int cardId);

        Task<IEnumerable<Checkout>> GetCheckoutsForAsset(int libraryAssetId);

        Task<IEnumerable<Checkout>> SearchCheckouts(string searchString);

        Task<Checkout> GetLatestCheckout(int id);

        Task<int> GetMemberCurrentCheckoutAmount(int cardId);

        Task<IEnumerable<Checkout>> GetMemberCurrentCheckouts(int cardId);

        Task<bool> IsAssetCurrentlyCheckedOutByMember(int assetId, int cardId);
    }
}