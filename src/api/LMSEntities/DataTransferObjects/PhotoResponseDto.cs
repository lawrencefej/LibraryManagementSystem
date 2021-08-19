using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class PhotoResponseDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
