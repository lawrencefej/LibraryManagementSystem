using System;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class DataDto
    {
        [Required]
        public int Count { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DayOfWeek? Day { get; set; }
        public int? Month { get; set; }
    }
}
