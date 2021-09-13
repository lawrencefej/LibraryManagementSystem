using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class TokenResponseDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
