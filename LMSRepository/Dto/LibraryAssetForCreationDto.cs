using LMSLibrary.Models;
using System;

namespace LMSRepository.Dto
{
    public class LibraryAssetForCreationDto
    {
        public LibraryAssetForCreationDto()
        {
            CopiesAvailable = NumberOfCopies;
            Added = DateTime.Now;
        }

        public string Title { get; set; }
        public int Year { get; set; }
        public DateTime Added { get; set; }

        //public Status Status { get; set; }
        public decimal Cost { get; set; }

        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
        public string Description { get; set; }

        //public AssetPhoto Photo { get; set; }
        //public AssetType AssetType { get; set; }
        public int AssetTypeId { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}