using LMSLibrary.Helpers;
using LMSLibrary.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMSLibrary.Dto
{
    public class ReserveForCreationDto
    {
        public int LibraryAssetId { get; set; }
        public int LibraryCardId { get; set; }
        public DateTime Reserved { get; set; }
        public bool IsCheckedOut { get; set; }
        public DateTime Until { get; set; }
        public Status Status { get; set; }

        [FeesPaid]
        public decimal Fees { get; set; }
        [Required]
        public string Status2 { get; set; }

        public ReserveForCreationDto()
        {
            Reserved = DateTime.Now;
            Until = DateTime.Today.AddDays(5);
        }
    }
}
