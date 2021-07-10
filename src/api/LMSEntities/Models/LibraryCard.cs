using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;

namespace LMSEntities.Models
{
    public class LibraryCard
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CardNumber { get; private set; }
        public decimal Fees { get; private set; } = 0;
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public AppUser Member { get; set; }
        public int MemberId { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public MemberGender Gender { get; set; }
        public LibraryCardStatus Status { get; set; } = LibraryCardStatus.Good;
        public LibraryCardPhoto LibraryCardPhoto { get; set; }
        public ICollection<Checkout> Checkouts { get; set; }
        public ICollection<ReserveAsset> ReservedAssets { get; set; }

        public bool DoesCardHaveFees()
        {
            return Fees > 0;
        }

        public void GenerateCardNumber()
        {
            string date = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            CardNumber = $"{LastName.Substring(0, 1).ToUpper()}-{date.Substring(0, 4)}-{date.Substring(4, 4)}-{date.Substring(8, 6)}";

        }

        public void AddFeesToCard(decimal fees)
        {
            Fees += fees;
        }

        public void ZeroFees()
        {
            Fees = 0;
        }
    }
}
