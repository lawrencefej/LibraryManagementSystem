using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Helpers
{
    public class AppSettings : IValidatable
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

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
    }
}
