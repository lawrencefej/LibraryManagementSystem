using System.ComponentModel.DataAnnotations;

namespace LMSService.Helpers
{
    public class AppSettings
    {
        [Required]
        [StringLength(50, MinimumLength = 16, ErrorMessage = "Token must be 16 characters or more")]
        public string Token { get; set; }

        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public string Port { get; set; }

        [Required]
        public string DbUser { get; set; }

        [Required]
        public string DbPassword { get; set; }

        [Required]
        public string DatabaseName { get; set; }

        public bool SeedDb { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 16, ErrorMessage = "Secret must be 16 characters or more")]
        public string Secret { get; set; }

        [Required]
        public double TokenLifetime { get; set; } = 60;

        [Required]
        public double RefreshTokenLifetime { get; set; } = 5;
    }
}
