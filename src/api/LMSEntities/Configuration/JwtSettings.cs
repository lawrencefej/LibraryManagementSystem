using System.ComponentModel.DataAnnotations;

namespace LMSEntities.Configuration
{
    public class JwtSettings
    {
        [Required]
        [StringLength(50, MinimumLength = 16, ErrorMessage = "Secret must be 16 characters or more")]
        public string Secret { get; set; }

        [Required]
        public double TokenLifetime { get; set; } = 60;

        [Required]
        public double RefreshTokenLifetime { get; set; } = 5;
    }
}
