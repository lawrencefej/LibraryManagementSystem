using System;
using LMSEntities.Models;

namespace LMSEntities.DataTransferObjects
{
    public class CheckoutForCreationDto
    {
        public int LibraryAssetId { get; set; }
        public int LibraryCardId { get; set; }
        public int UserId { get; set; }
        public DateTime CheckoutDate { get; set; } = DateTime.Today;
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(21);
        // public Status Status { get; set; }
        public CheckoutStatus Status { get; set; } = CheckoutStatus.Checkedout;
        // public int StatusId { get; set; }
        public decimal Fees { get; set; }
        public string AssetStatus { get; set; }
        public int CurrentCheckoutCount { get; set; }

        //public IEnumerable<LibraryAssetForCreationDto> Assets { get; set; }
        public LibraryAssetForDetailedDto Asset { get; set; }

        public CheckoutForCreationDto()
        {
            // StatusId = (int)StatusEnum.Checkedout;
            // Status = CheckoutStatus.Checkedout;
            // CheckoutDate = DateTime.Today;
            // DueDate = DateTime.Today.AddDays(21);
        }
    }
}
