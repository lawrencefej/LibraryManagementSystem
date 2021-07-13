using System.Collections.Generic;
using LMSEntities.DataTransferObjects;

namespace LMSEntities.Models
{
    public class Basket
    {
        public int Id { get; set; }

        // public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();

        public int LibraryCardId { get; set; }

        // public DateTime ExpiryDate { get; set; }

        // public bool IsExpired { get; set; }

        public IList<LibraryAssetForBasket> Assets { get; set; } = new List<LibraryAssetForBasket>();
    }
}
