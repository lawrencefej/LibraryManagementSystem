using System;

namespace LMSLibrary.Dto
{
    public class UserForUpdateDto
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}