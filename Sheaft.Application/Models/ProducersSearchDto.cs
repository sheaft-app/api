using System.Collections.Generic;
using Sheaft.Domain;

namespace Sheaft.Application.Models
{

    public class ProducersSearchDto
    {
        public long Count { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
    }
}