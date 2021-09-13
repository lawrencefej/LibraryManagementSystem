using System;
using System.Collections.Generic;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForCreationDto
    {
        public string DeweyIndex { get; set; }

        public LibraryAssetTypeDto AssetType { get; set; }

        public ICollection<LibraryAssetAuthorDto> AssetAuthors { get; set; }

        public ICollection<LibraryAssetCategoryDto> AssetCategories { get; set; }

        public int NumberOfCopies { get; set; }

        public string Description { get; set; }

        public string ISBN { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }
    }
}
