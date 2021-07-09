using System;
using System.Collections.Generic;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForCreationDto
    {
        public string DeweyIndex { get; set; }

        public LibraryAssetTypeDto AssetType { get; set; }

        public ICollection<LibraryAssetAuthorDto> AssetAuthors { get; set; }

        public ICollection<LibraryAssetCategoryDto> AssetCategories { get; set; }

        public int NumberOfCopies { get; set; }

        public string Description { get; set; }

        public string ISBN { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        // [Required]
        // public DateTime DateOfBirth { get; set; }

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if (AssetType == LibraryAssetTypeDto.Book && string.IsNullOrEmpty(Isbn))
        //     {
        //         yield return new ValidationResult($"ISBN is required when the type is {LibraryAssetTypeDto.Book}", new[] { nameof(Isbn) });
        //     }
        // }
    }
}
