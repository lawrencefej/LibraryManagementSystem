using System.ComponentModel.DataAnnotations;

namespace LMSEntities.Configuration
{
    public class DbSettings
    {
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


        public string GetConnectionString()
        {
            return $"Server={Host};Port={Port};Database={DatabaseName};Uid={DbUser};Pwd={DbPassword};";
        }
    }
}
