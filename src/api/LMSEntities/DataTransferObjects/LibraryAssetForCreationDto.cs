using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LMSEntities.Models;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForCreationDto
    {
        public DateTime Added { get; set; } = DateTime.Today;
        public string DeweyIndex { get; set; }
        [Required]
        public LibraryAssetTypeDto AssetType { get; set; }

        [Required]
        public ICollection<LibraryAssetAuthorDto> AssetAuthors { get; set; }

        [Required]
        public ICollection<LibraryAssetCategoryDto> AssetCategories { get; set; }

        public int CopiesAvailable { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public int NumberOfCopies { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }
    }

    public enum LibraryAssetTypeDto
    {
        Book = 1,
        Media = 2,
        Other = 3
    }

    public enum LibraryAssetStatusDto
    {
        Available = 1,
        Unavailable = 2
    }
}
