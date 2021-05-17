using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<LibraryAssetAuthor> AuthorAssets { get; set; }
    }
}
