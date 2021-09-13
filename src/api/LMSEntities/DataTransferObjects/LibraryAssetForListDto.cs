using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForListDto
    {

        [Required]
        public int Id { get; set; }

        [Required]

        public string Title { get; set; }

        [Required]

        public int Year { get; set; }

        [Required]

        public string AssetType { get; set; }

        [Required]

        public string AuthorName { get; set; }

        [Required]

        public int CopiesAvailable { get; set; }

        [Required]

        public string Status { get; set; }
    }
}
