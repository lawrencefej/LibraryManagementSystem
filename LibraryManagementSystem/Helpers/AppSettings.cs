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

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
    }
}