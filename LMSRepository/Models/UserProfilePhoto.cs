using System;
using System.Collections.Generic;
using System.Text;

namespace LMSLibrary.Models
{
    public class UserProfilePhoto : Photo
    {
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
