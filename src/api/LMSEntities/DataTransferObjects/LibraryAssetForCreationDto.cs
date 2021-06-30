using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForCreationDto
    {
        public DateTime Added { get; private set; } = DateTime.Today;
        public string DeweyIndex { get; set; }
        [Required]
        public LibraryAssetTypeDto AssetType { get; set; }

        [Required]
        public ICollection<LibraryAssetAuthorDto> AssetAuthors { get; set; }

        [Required]
        public ICollection<LibraryAssetCategoryDto> AssetCategories { get; set; }

        public LibraryAssetStatusDto Status { get; private set; } = LibraryAssetStatusDto.Available;

        [Required]
        public int NumberOfCopies { get; set; }
        public string Description { get; set; }

        public string Isbn { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
