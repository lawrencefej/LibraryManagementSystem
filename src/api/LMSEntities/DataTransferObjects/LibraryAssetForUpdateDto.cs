using System.Collections.Generic;
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
        [Required]
        public int NumberOfCopies { get; set; }

        [Required]
        public int CopiesAvailable { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public LibraryAssetTypeDto AssetType { get; set; }
        public string ISBN { get; set; }
        public string DeweyIndex { get; set; }

        // [Required]
        public ICollection<LibraryAssetAuthorDto> AssetAuthors { get; set; }

        // [Required]
        public ICollection<LibraryAssetCategoryDto> AssetCategories { get; set; }
    }
}
