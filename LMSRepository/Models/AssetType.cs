namespace LMSRepository.Interfaces.Models
{
    public class AssetType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public ICollection<LibraryAsset> LibraryAssets { get; set; }
        //public int LibraryAssetId { get; set; }
    }
}