namespace LMSEntities.Models
{
    public class CheckoutItem
    {
        public int Id { get; set; }
        public LibraryAsset LibraryAsset { get; set; }
        public int LibraryAssetId { get; set; }
        public CheckoutStatus Status { get; set; }
        public Checkout Checkout { get; set; }
        public int CheckoutId { get; set; }
    }
}
