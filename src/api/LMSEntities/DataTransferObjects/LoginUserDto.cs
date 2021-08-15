namespace LMSEntities.DataTransferObjects
{
    public class LoginUserDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoUrl { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
