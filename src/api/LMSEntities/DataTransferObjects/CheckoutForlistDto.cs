using System;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class CheckoutForListDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public DateTime CheckoutDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? DateReturned { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int LibraryAssetId { get; set; }

    }
}
