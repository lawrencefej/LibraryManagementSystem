namespace LMSEntities.DataTransferObjects
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
    }
}
