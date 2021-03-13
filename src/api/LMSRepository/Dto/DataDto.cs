using System;

namespace LMSRepository.Dto
{
    public class DataDto
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DayOfWeek? Day { get; set; }
        public int? Month { get; set; }
    }
}