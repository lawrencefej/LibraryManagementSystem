using System;

namespace LMSEntities.DataTransferObjects
{
    public class LibrarycardForListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CardNumber { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string LibraryCardPhoto { get; set; }

    }
}
