using LMSEntities.Models;

namespace LMSEntities.DataTransferObjects
{
    public class CheckoutItemForCreationDto
    {
        public int LibraryAssetId { get; set; }
        public CheckoutStatusDto Status { get; set; } = CheckoutStatusDto.Checkedout;
        public int CheckoutId { get; set; }
    }
}
