using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMSEntities.DataTransferObjects
{
    public class ChartDto
    {
        [Required]
        public List<DataDto> Data { get; set; } = new List<DataDto>();

        [Required]
        public string Label { get; set; }
    }
}
