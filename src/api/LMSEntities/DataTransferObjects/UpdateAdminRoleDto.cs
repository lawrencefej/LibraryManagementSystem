using System.ComponentModel.DataAnnotations;
using LMSEntities.Enumerations;

namespace LMSEntities.DataTransferObjects
{
    public class UpdateAdminRoleDto
    {
        public int Id { get; set; }

        public UserRoles Role { get; set; }
    }
}
