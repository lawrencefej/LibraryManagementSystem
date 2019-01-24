using LMSLibrary.Models;
using System;

namespace LMSLibrary.Dto
{
    public class CheckoutForReturnDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public int LibraryCardNumber { get; set; }
        public DateTime Since { get; set; } /*= DateTime.Now;*/
        public DateTime Until { get; set; }
        public string Status { get; set; }
        public DateTime? DateReturned { get; set; }
        //public int LibraryAssetId { get; set; }
        //public int LibraryCardId { get; set; }
    }
}
