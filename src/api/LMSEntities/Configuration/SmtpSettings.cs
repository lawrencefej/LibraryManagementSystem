using System.ComponentModel.DataAnnotations;

namespace LMSEntities.Configuration
{
    public class SmtpSettings
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string FromEmail { get; set; }

        [Required]
        public string FromName { get; set; }
    }
}
