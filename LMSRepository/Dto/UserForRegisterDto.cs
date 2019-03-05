using System;
using System.ComponentModel.DataAnnotations;

namespace LMSRepository.Interfaces.Dto
{
    public class UserForRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        //[Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 9 characters")]
        public string Password { get; set; }

        //[Required]
        public string Gender { get; set; }

        //[Required]
        public string Address { get; set; }

        //[Required]
        public DateTime DateOfBirth { get; set; }

        //[Required]
        public string City { get; set; }

        //[Required]
        public string State { get; set; }

        //[Required]
        public string Zipcode { get; set; }

        //[Required]
        public DateTime Created { get; set; }

        public UserForRegisterDto()
        {
            UserName = Email;
            Created = DateTime.Now;
        }
    }
}