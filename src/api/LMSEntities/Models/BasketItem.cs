namespace LMSEntities.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int LibraryAssetId { get; set; }
        public string AssetName { get; set; }
        public string PictureUrl { get; set; }
        // public LibraryAsset LibraryAsset { get; set; }
    }
}
