using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class ProducersSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<ProducerDto> Producers { get; set; }
    }
}