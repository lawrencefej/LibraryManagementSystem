using LMSRepository.Models;
using System.Collections.Generic;

namespace LMSRepository.Interfaces
{
    public interface ICheckoutHistory
    {
        List<CheckoutHistory> GetAllCheckoutHistory();

        List<CheckoutHistory> GetForAsset(LibraryAsset asset);

        IEnumerable<CheckoutHistory> GetForCard(LibraryCard card);

        CheckoutHistory Get(int id);

        void Add(CheckoutHistory newCheckoutHistory);
    }
}