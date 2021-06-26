using System;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryCardForCreationDto
    {
        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        // public string CardNumber { get; set; }
        public decimal Fees { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.Today;
        // public AppUser Member { get; set; }
        // public int MemberId { get; set; }
        [Required]
        public AddressDto Address { get; set; }
        // public int AddressId { get; set; }
        [Required]
        public MemberGenderDto Gender { get; set; }
        [Required]
        public LibraryCardStatusDto Status { get; set; } = LibraryCardStatusDto.Good;
    }
}
