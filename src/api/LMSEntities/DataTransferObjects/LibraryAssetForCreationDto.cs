using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LMSEntities.Models;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForCreationDto
    {
        //public int Id { get; set; }
        public DateTime Added { get; set; } = DateTime.Today;
        public string DeweyIndex { get; set; }

        //[Required]
        // [Range(1, int.MaxValue, ErrorMessage = "Please select a valid author")]
        // public int AssetTypeId { get; set; }

        // public AssetTypeDto AssetType { get; set; }
        [Required]
        public LibraryAssetTypeDto AssetType { get; set; }

        // public int AuthorId { get; set; }

        [Required]
        public ICollection<AuthorDto> AssetAuthors { get; set; } = new List<AuthorDto>();

        // [Required]
        // public int CategoryId { get; set; }

        [Required]
        public ICollection<CategoryDto> Categories { get; set; }

        public int CopiesAvailable { get; set; }
        // public decimal Cost { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public int NumberOfCopies { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }

        // public LibraryAssetForCreationDto()
        // {
        //     Added = DateTime.Today;
        // }
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
