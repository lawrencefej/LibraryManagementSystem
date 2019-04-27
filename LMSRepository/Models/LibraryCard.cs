using System;
using System.Collections.Generic;

namespace LMSRepository.Models
{
    public class LibraryCard
    {
        public int Id { get; set; }
        public int CardNumber { get; set; }
        public decimal Fees { get; set; }
        public DateTime Created { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<Checkout> Checkouts { get; set; }
        public ICollection<ReserveAsset> ReservedAssets { get; set; }
    }
}