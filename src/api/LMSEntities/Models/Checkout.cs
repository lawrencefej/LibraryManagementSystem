using System;
using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public ICollection<CheckoutItem> Items { get; set; }
        public LibraryCard LibraryCard { get; set; }
        public int LibraryCardId { get; set; }
        public string LibraryCardNumber { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public byte RenewalCount { get; set; } = 1;
        public bool IsReturned { get; set; }
        public DateTime? DateReturned { get; set; }
        public CheckoutStatus Status { get; set; }
    }
}
