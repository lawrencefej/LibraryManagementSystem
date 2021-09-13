using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class AddAuthorDto
    {
        [Required]
        public string FullName { get; set; }

        public string Description { get; set; }
    }
}
