using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LMSEntities.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Created { get; set; }

        public UserProfilePhoto ProfilePicture { get; set; }

        public LibraryCard LibraryCard { get; set; }

        public LmsGender? Gender { get; set; }

        public bool IsAccountActivated { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
