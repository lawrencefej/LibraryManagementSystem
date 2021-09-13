using System.ComponentModel.DataAnnotations;

namespace LMSEntities.Configuration
{
    public class SendGridSettings
    {
        [Required]
        public string User { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string FromEmail { get; set; }

        [Required]
        public string FromName { get; set; }
    }
}
