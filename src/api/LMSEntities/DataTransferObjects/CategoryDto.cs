using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
