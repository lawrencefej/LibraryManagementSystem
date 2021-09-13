using System;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.Models
{
    public class RefreshToken
    {
        public AppUser User { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string JwtId { get; set; }
        public string RequestIp { get; set; }
        public string Token { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }

    }
}
