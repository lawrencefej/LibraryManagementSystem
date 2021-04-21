using System;

namespace LMSEntities.Models
{
    public abstract class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        //public User User { get; set; }
        //public int UserId { get; set; }
    }
}
