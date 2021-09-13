using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class AdminUserForListDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public UserRoleDto Role { get; set; }
    }
}
