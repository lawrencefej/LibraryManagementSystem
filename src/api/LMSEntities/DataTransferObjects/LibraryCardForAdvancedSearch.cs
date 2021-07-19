using System;

namespace LMSEntities.DataTransferObjects
{
    public class LibraryCardForAdvancedSearch
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Zipcode { get; set; }
    }
}
