using System.Collections.Generic;

namespace LMSEntities.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<LibraryAssetCategory> CategoryAssets { get; set; }
    }
}
