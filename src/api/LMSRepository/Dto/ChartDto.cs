using System.Collections.Generic;

namespace LMSRepository.Dto
{
    public class ChartDto
    {
        public List<DataDto> Data { get; set; }
        public string Label { get; set; }

        public ChartDto()
        {
            Data = new List<DataDto>();
        }
    }
}