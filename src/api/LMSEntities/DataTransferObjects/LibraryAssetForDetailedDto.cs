using System;
using System.Collections.Generic;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryAssetForDetailedDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime Added { get; set; }
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
        public string Description { get; set; }
        public string AssetType { get; set; }
        public List<AuthorDto> Authors { get; set; }
        public string ISBN { get; set; }
        public string DeweyIndex { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }
}
