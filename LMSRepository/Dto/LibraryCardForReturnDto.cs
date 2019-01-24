using System;
using System.Collections.Generic;
using System.Text;

namespace LMSLibrary.Dto
{
    public class LibraryCardForReturnDto
    {
        public int Id { get; set; }
        public int CardNumber { get; set; }
        public decimal Fees { get; set; }
    }
}
