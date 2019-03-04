using System.Collections.Generic;

namespace LMSRepository.Interfaces.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<LibraryAsset> Assets { get; set; }
    }
}