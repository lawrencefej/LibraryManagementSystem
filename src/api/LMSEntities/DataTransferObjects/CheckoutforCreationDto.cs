using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LMSEntities.Models;

namespace LMSEntities.DataTransferObjects
{
    public class CheckoutForCreationDto
    {
        [Required]
        public ICollection<CheckoutItem> Items { get; set; }
        // TODO IS this needed
        [Required]
        public int LibraryCardId { get; set; }
        [Required]
        public string LibraryCardNumber { get; set; }
        public DateTime CheckoutDate { get; set; } = DateTime.Today;
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(21);
        public CheckoutStatusDto Status { get; set; } = CheckoutStatusDto.Checkedout;
        public byte RenewalCount { get; set; } = 1;
        public bool IsReturned { get; set; }
    }
}
