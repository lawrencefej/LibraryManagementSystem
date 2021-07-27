using System;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibrarycardForListDto
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
        public string Gender { get; set; }

        [Required]
        public string Status { get; set; }
        public string LibraryCardPhoto { get; set; }

    }
}
