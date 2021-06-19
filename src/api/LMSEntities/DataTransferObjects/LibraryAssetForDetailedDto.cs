using System;
using System.Collections.Generic;
using LMSEntities.Models;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForDetailedDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public LibraryAssetStatus Status { get; set; }
        // public string Status { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Cost { get; set; }
        public DateTime Added { get; set; }
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
        public string Description { get; set; }

        //public string AssetType { get; set; }
        // public AssetTypeDto AssetType { get; set; }
        public LibraryAssetType AssetType { get; set; }

        // public string AuthorName { get; set; }
        public string AuthorName { get; set; }
        public List<AuthorDto> Authors { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int AssetTypeId { get; set; }
        public string ISBN { get; set; }
        public string DeweyIndex { get; set; }

        //public string Category { get; set; }
        public CategoryDto Category { get; set; }
    }
}
