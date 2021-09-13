using System;

namespace LMSEntities.Models
{
    public class Checkout
    {
        private const int MaxRenewCount = 3;

        public int Id { get; set; }

        public LibraryAsset LibraryAsset { get; set; }

        public int LibraryAssetId { get; set; }

        public LibraryCard LibraryCard { get; set; }

        public int LibraryCardId { get; set; }

        public DateTime CheckoutDate { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(21);

        public DateTime DateReturned { get; set; }

        public CheckoutStatus Status { get; set; } = CheckoutStatus.Checkedout;

        public byte RenewalCount { get; private set; }

        public void RenewCheckout()
        {
            RenewalCount++;
            if (RenewalCount > MaxRenewCount)
            {
                throw new ArgumentOutOfRangeException($"Items cannot be renewed more than {MaxRenewCount} times.");
            }

            DueDate = DueDate.AddDays(21);
        }

        public void CheckInAsset()
        {
            DateReturned = DateTime.UtcNow;
            Status = CheckoutStatus.Returned;
            LibraryAsset.IncreaseCopiesAvailable();
        }
    }
}
