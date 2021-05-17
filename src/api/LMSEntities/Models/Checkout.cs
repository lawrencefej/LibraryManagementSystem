using System;
using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public List<CheckoutItem> Items { get; set; } = new List<CheckoutItem>();
        public LibraryCard LibraryCard { get; set; }
        public int LibraryCardId { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public byte RenewalCount { get; set; } = 1;
        public bool IsReturned { get; set; }
        public DateTime? DateReturned { get; set; }

        public CheckoutStatus Status { get; set; }
        // public Status Status { get; set; }
        // public int StatusId { get; set; }
    }

    public enum CheckoutStatus
    {
        Checkedout = 1,
        Returned = 2,
        Late = 3
    }
}
