using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Configuration.Entities
{
    public class CloudinarySettings
    {
        [Required]
        public string CloudName { get; set; }

        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string ApiSecret { get; set; }
    }
}
