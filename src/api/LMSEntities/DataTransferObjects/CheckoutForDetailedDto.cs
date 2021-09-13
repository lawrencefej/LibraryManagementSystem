using System;

namespace LMSEntities.DataTransferObjects
{
    public class CheckoutForDetailedDto
    {
        public int Id { get; set; }
        public LibraryAssetForListDto LibraryAsset { get; set; }
        public LibraryCardForDetailedDto LibraryCard { get; set; }
        public DateTime CheckoutDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}
