namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetAuthorDto
    {
        public int Id { get; set; }
        public int LibrayAssetId { get; set; }
        public int AuthorId { get; set; }
        public byte Order { get; set; }
    }
}
