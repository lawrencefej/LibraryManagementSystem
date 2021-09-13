using System;
using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class LibraryAsset
    {
        public int Id { get; set; }
        public DateTime Added { get; set; } = DateTime.UtcNow;
        public string DeweyIndex { get; set; }
        public ICollection<LibraryAssetCategory> AssetCategories { get; set; }
        public LibraryAssetStatus Status { get; set; } = LibraryAssetStatus.Available;
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; private set; }
        public string Description { get; set; }
        public AssetPhoto Photo { get; set; }
        public LibraryAssetType AssetType { get; set; }
        public ICollection<LibraryAssetAuthor> AssetAuthors { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }

        public void IncreaseCopiesAvailable()
        {
            CopiesAvailable++;

            if (Status == LibraryAssetStatus.Unavailable)
            {
                Status = LibraryAssetStatus.Available;
            }
        }

        public void ReduceCopiesAvailable()
        {
            CopiesAvailable--;

            if (CopiesAvailable == 0)
            {
                Status = LibraryAssetStatus.Unavailable;
            }
        }

        public void SetToAvailable()
        {
            if (CopiesAvailable > 0)
            {
                Status = LibraryAssetStatus.Available;
            }
        }

        public void SetCopiesAvailable()
        {
            CopiesAvailable = NumberOfCopies;
        }
    }
}
