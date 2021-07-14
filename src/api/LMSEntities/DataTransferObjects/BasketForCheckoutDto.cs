using System.Collections.Generic;

namespace LMSEntities.DataTransferObjects
{
    public class BasketForCheckoutDto
    {
        // public int Id { get; set; }

        public int LibraryCardId { get; set; }

        public IList<LibraryAssetForBasket> Assets { get; set; } = new List<LibraryAssetForBasket>();
    }
}
