using LMSRepository.Helpers;
using LMSRepository.Models;
using System;

namespace LMSRepository.Dto
{
    public class CheckoutForCreationDto
    {
        public int LibraryAssetId { get; set; }
        public int LibraryCardId { get; set; }
        public int userId { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public Status Status { get; set; }
        public int StatusId { get; set; }
        public decimal Fees { get; set; }
        public string AssetStatus { get; set; }
        public int CurrentCheckoutCount { get; set; }

        //public IEnumerable<LibraryAssetForCreationDto> Assets { get; set; }
        public LibraryAssetForDetailedDto Asset { get; set; }

        public CheckoutForCreationDto()
        {
            StatusId = (int)EnumStatus.Checkedout;
            Since = DateTime.Today;
            Until = DateTime.Today.AddDays(21);
        }
    }
}