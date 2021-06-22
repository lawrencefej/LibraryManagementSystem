using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        public int StatusId { get; set; }
        [Required]
        public int NumberOfCopies { get; set; }

        [Required]
        public int CopiesAvailable { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public LibraryAssetTypeDto AssetTypeId { get; set; }
        public string ISBN { get; set; }
        public string DeweyIndex { get; set; }
    }
}
