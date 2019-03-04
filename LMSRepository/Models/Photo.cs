using System;

namespace LMSRepository.Interfaces.Models
{
    public abstract class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime DateAdded { get; set; }
        //public User User { get; set; }
        //public int UserId { get; set; }
    }
}