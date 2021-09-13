using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
