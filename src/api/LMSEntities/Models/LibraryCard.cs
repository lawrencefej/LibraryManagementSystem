using System;
using System.Collections.Generic;
using System.Globalization;

namespace LMSEntities.Models
{
    public class LibraryCard
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CardNumber { get; set; }
        public decimal Fees { get; set; }
        public DateTime Created { get; set; }
        public AppUser Member { get; set; }
        public int MemberId { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public MemberGender Gender { get; set; }
        public LibraryCardStatus Status { get; set; }
        public LibraryCardPhoto LibraryCardPhoto { get; set; }
        public ICollection<Checkout> Checkouts { get; set; }
        public ICollection<ReserveAsset> ReservedAssets { get; set; }
    }
}
