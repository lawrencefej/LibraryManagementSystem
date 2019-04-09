namespace LMSRepository.Dto
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
    }
}