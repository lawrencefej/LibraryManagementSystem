using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryCardForDetailedDto
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
        public string CardNumber { get; set; }

        [Required]
        public decimal Fees { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public AddressDto Address { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string PhotoUrl { get; set; }

        [Required]
        public IList<CheckoutForListDto> Checkouts { get; set; } = new List<CheckoutForListDto>();

    }
}
