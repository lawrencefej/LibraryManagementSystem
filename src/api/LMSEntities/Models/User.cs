using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LMSEntities.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public UserProfilePhoto ProfilePicture { get; set; }
        public LibraryCard LibraryCard { get; set; }

        //public int? LibraryCardId { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
