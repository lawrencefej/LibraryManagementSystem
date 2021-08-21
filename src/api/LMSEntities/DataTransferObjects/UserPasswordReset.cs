using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class UserPasswordResetRequest
    {

        [Required]
        public int UserId { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
