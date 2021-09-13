using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class UserForDetailedDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { get; set; }
        // public string Age { get; set; }
        // public DateTime Created { get; set; }

        [Required]
        public DateTime LastActive { get; set; }

        [Required]
        public DateTime Created { get; set; }
        // public string Address { get; set; }
        // public string City { get; set; }
        // public string State { get; set; }
        // public string Zipcode { get; set; }
        public string PhotoUrl { get; set; }
        // public int LibraryCardNumber { get; set; }
        // public decimal Fees { get; set; }
        [Required]
        public string Role { get; set; }
        // public string Token { get; set; }
        // public ICollection<UserRoleDto> UserRoles { get; set; }
    }
}
