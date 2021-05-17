using System;
using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public int LibraryCardId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
    }
}
