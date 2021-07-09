using System;
using System.Collections.Generic;

namespace LMSEntities.DataTransferObjects
{
    public class CheckoutForReturnDto
    {
        public int Id { get; set; }
        public ICollection<CheckoutItemForReturn> Items { get; set; }
        public string Title { get; set; }
        public string LibraryCardNumber { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public string Status { get; set; }
        public DateTime? DateReturned { get; set; }
        public int LibraryAssetId { get; set; }
        public int LibraryCardId { get; set; }
    }
}
