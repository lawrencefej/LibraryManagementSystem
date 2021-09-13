namespace LMSEntities.Models
{
    public class UserProfilePhoto : Photo
    {
        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
