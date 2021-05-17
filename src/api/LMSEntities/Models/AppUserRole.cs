using Microsoft.AspNetCore.Identity;

namespace LMSEntities.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        // public User User { get; set; }
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
