namespace LMSEntities.Models
{
    public class LibraryAssetAuthor
    {
        // public int Id { get; set; }
        public int LibrayAssetId { get; set; }
        public LibraryAsset LibraryAsset { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public byte Order { get; set; }
    }
}
