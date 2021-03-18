using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class SetResourceIdsAvailabilityDto
    {
        public IEnumerable<Guid> Ids { get; set; }
        public bool Available { get; set; }
    }
}