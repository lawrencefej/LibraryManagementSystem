namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int StatusId { get; set; }
        public int NumberOfCopies { get; set; }

        //public AuthorDto Author { get; set; }
        public int CopiesAvailable { get; set; }

        public string Description { get; set; }
        public int AssetTypeId { get; set; }
        public int AuthorId { get; set; }
        public string ISBN { get; set; }
        public string DeweyIndex { get; set; }
        public int CategoryId { get; set; }
    }
}
