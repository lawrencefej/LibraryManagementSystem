namespace LMSEntities.Models
{
    public class LibraryCardPhoto : Photo
    {
        public LibraryCard LibraryCard { get; set; }
        public int LibraryCardId { get; set; }
    }
}
