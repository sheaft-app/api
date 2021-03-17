using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Dto
{

    public class ProducersSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<ProducerDto> Producers { get; set; }
    }
}