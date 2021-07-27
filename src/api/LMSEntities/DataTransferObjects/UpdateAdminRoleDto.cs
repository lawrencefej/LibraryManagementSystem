using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class UpdateAdminRoleDto
    {
        public int Id { get; set; }

        public string Role { get; set; }
    }
}
