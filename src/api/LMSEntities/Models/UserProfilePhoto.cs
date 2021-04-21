namespace LMSEntities.Models
{
    public class UserProfilePhoto : Photo
    {
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
