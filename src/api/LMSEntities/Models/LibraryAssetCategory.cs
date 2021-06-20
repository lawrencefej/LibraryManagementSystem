namespace LMSEntities.Models
{
    public class LibraryAssetCategory
    {
        public int LibrayAssetId { get; set; }
        public LibraryAsset LibraryAsset { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
