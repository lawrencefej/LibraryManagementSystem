using System;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForCreationDto
    {
        //public int Id { get; set; }
        public DateTime Added { get; set; }

        //[Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid author")]
        public int AssetTypeId { get; set; }

        public AssetTypeDto AssetType { get; set; }

        public int AuthorId { get; set; }

        //[Required]
        public AuthorDto Author { get; set; }

        //[Required]
        public int CategoryId { get; set; }

        //[Required]
        public CategoryDto Category { get; set; }

        public int CopiesAvailable { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public int NumberOfCopies { get; set; }

        [Required]
        public string Title { get; set; }

        public int Year { get; set; }

        public LibraryAssetForCreationDto()
        {
            Added = DateTime.Today;
        }
    }
}
