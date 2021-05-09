using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class LibraryUser : AppUser
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
