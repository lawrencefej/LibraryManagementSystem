using System.ComponentModel.DataAnnotations;

namespace LMSEntities.Configuration
{
    public class CloudinarySettings
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string ApiSecret { get; set; }
    }
}
