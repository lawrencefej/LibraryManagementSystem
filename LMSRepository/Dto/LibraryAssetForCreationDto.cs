using System;
using System.ComponentModel.DataAnnotations;

namespace LMSRepository.Dto
{
    public class LibraryAssetForCreationDto
    {
        public DateTime Added { get; set; }

        [Required]
        public int AssetTypeId { get; set; }

        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

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
            //CopiesAvailable = NumberOfCopies;
            Added = DateTime.Today;
        }
    }
}