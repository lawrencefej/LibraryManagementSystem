using LMSLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSLibrary.Data
{
    public interface ICheckoutRepository2
    {
        Task<List<Checkout>> GetAllCheckouts();
        Task<Checkout> GetCheckout(int id);
        void Add(Checkout newCheckout);
        Task<List<CheckoutHistory>> GetCheckoutHistory(int id);
        void ReserveAsset(int assetId, int libraryCardId);
        void CheckoutAsset(int assetId, int libraryCardId);
        void CheckInAsset(int id, int libraryCardId);
        Task<Checkout> GetLatestCheckout(int id);
        //int GetNumberOfCopies(int id);
        bool IsCheckedOut(int id);

        string GetCurrentHoldPatron(int id);
        string GetCurrentHoldPlaced(int id);
        string GetCurrentPatron(int id);
        Task<List<Hold>> GetCurrentHolds(int id);
    }
}
