using System;

namespace LMSRepository.Dto
{
    public class LibraryCardForCreationDto
    {
        public int CardNumber { get; set; }
        public decimal Fees { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }

        public LibraryCardForCreationDto()
        {
            Created = DateTime.Now;
            Fees = 0;
        }
    }
}