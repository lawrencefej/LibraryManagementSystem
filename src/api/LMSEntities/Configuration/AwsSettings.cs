using System.ComponentModel.DataAnnotations;

namespace LMSEntities.Configuration
{
    public class AwsSettings
    {
        [Required]
        public string Test { get; set; }
    }
}
