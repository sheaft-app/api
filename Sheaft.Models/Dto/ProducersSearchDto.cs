using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{

    public class ProducersSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<ProducerDto> Producers { get; set; }
    }
}