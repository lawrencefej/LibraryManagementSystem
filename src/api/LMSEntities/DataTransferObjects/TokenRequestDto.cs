using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class TokenRequestDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
