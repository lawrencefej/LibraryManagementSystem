namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string AssetType { get; set; }
        public string AuthorName { get; set; }
        public int CopiesAvailable { get; set; }
    }
}
