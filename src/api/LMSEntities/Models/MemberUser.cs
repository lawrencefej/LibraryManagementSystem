namespace LMSEntities.Models
{
    public class MemberUser : AppUser
    {
        public LibraryCard LibraryCard { get; set; }
        public bool IsAccountActivated { get; set; }
        // public int LibraryCardId { get; set; }

    }
}
