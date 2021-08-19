using System;

namespace LMSEntities.Models
{
    public abstract class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        // Set datetime to utc now
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public string PublicId { get; set; }
    }
}
