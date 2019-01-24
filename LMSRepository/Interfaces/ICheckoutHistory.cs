using LMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMSLibrary.Data
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
