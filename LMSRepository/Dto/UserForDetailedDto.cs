using LMSRepository.Dto;
using System;
using System.Collections.Generic;

namespace LMSRepository.Dto
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string PhotoUrl { get; set; }
        public int LibraryCardNumber { get; set; }
        public decimal Fees { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public ICollection<UserRoleDto> UserRoles { get; set; }
    }
}