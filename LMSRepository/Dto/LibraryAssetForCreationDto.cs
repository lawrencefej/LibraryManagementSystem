using LMSLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMSLibrary.Dto
{
    public abstract class LibraryAssetForCreationDto
    {
        public LibraryAssetForCreationDto()
        {
            CopiesAvailable = NumberOfCopies;
            Added = DateTime.Now;
        }
        public string Title { get; set; }
        public int Year { get; set; }
        public DateTime Added { get; set; }
        public Status Status { get; set; }
        public decimal Cost { get; set; }
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
        public string Description { get; set; }
        public AssetPhoto Photo { get; set; }
        public AssetType AssetType { get; set; }
    }
}
