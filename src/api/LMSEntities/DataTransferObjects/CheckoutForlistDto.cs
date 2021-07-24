using System;

namespace LMSEntities.DataTransferObjects
{
    public class CheckoutForListDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string CardNumber { get; set; }

        public DateTime CheckoutDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? DateReturned { get; set; }

        public string Status { get; set; }

        public int LibraryAssetId { get; set; }

    }
}
