namespace LMSEntities.DataTransferObjects
{
    public class AdminUserForListDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public UserRoleDto Role { get; set; }
    }
}
