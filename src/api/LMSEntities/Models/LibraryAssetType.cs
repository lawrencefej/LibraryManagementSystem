namespace LMSEntities.Models
{
    public class LibraryAssetType
    {
        public int Id { get; set; }
        public LibraryAsset LibraryAsset { get; set; }
        public AssetType AssetType { get; set; }
        public int LibraryAssetId { get; set; }
        public int AssetTypeId { get; set; }
    }
}
