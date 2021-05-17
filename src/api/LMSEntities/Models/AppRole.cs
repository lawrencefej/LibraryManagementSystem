using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LMSEntities.Models
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
