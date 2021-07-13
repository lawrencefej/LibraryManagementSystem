using System;

namespace LMSEntities.DataTransferObjects
{
    public class CheckoutForListDto
    {
        public int Id { get; set; }

        public string CardNumber { get; set; }

        public int ItemCount { get; set; }

        public DateTime CheckoutDate { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; }

    }
}
