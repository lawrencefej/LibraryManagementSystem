using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetCategoryDto
    {
        public int LibrayAssetId { get; set; }
        [Required]
        public int CategoryId { get; set; }

    }
}
