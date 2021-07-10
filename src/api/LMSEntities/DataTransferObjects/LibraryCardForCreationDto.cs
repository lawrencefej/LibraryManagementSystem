using System;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryCardForCreationDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public AddressDto Address { get; set; }
        [Required]
        public MemberGenderDto Gender { get; set; }
    }
}
