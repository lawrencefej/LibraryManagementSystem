using System;
using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class LibraryCard
    {
        public int Id { get; set; }
        public int CardNumber { get; set; }
        public decimal Fees { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Now;
        public MemberUser Member { get; set; }
        public int MemberId { get; set; }
        public Address Address { get; set; }
        public ICollection<Checkout> Checkouts { get; set; }
        public ICollection<ReserveAsset> ReservedAssets { get; set; }
    }
}
