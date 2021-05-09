using System;
using Microsoft.AspNetCore.Identity;

namespace LMSEntities.Models
{
    public abstract class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; }
        public UserProfilePhoto ProfilePicture { get; set; }
    }
}
