using System.Collections.Generic;

namespace LMSEntities.DataTransferObjects
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
