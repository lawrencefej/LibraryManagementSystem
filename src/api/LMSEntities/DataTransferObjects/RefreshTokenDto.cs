using System;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public string JwtId { get; set; }

        [Required]
        public string RequestIp { get; set; }

        public int UserId { get; set; }
    }
}
