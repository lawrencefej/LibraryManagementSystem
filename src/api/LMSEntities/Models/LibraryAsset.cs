using System;
using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class LibraryAsset
    {
        public int Id { get; set; }
        public DateTime Added { get; set; }
        public string DeweyIndex { get; set; }
        public ICollection<Category> Categories { get; set; }
        // public Status Status { get; set; }
        // public int StatusId { get; set; }
        public LibraryAssetStatus Status { get; set; }
        // public decimal Cost { get; set; }
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
        public string Description { get; set; }
        public AssetPhoto Photo { get; set; }
        // public int AssetTypeId { get; set; }
        // public AssetType AssetType { get; set; }
        public LibraryAssetType AssetType { get; set; }
        // public Author Author { get; set; }
        // public int AuthorId { get; set; }
        public ICollection<LibraryAssetAuthor> AssetAuthors { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        // public int CategoryId { get; set; }
    }

    public enum LibraryAssetType
    {
        Book = 1,
        Media = 2,
        Other = 3
    }

    public enum LibraryAssetStatus
    {
        Available = 1,
        Unavailable = 2
    }
}
