using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForDetailedDto
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string PhotoUrl { get; set; }
        public DateTime Added { get; set; }

        [Required]
        public int NumberOfCopies { get; set; }

        [Required]
        public int CopiesAvailable { get; set; }
        public string Description { get; set; }

        [Required]
        public string AssetType { get; set; }

        [Required]
        public List<AuthorDto> Authors { get; set; }
        public string ISBN { get; set; }
        public string DeweyIndex { get; set; }

        [Required]
        public List<CategoryDto> Categories { get; set; }
    }
}
