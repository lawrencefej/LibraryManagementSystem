using LMSLibrary.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMSLibrary.Dto
{
    public class CheckoutForCreationDto
    {
        public int LibraryAssetId { get; set; }
        public int LibraryCardId { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public Status Status { get; set; }
        public decimal Fees { get; set; }
        public string AssetStatus { get; set; }
        public int CurrentCheckoutCount { get; set; }

        public CheckoutForCreationDto()
        {
            Since = DateTime.Now;
            Until = DateTime.Today.AddDays(21);
        }
    }
}
