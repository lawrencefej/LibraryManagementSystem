using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class StateDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviations { get; set; }
    }
}
