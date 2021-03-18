using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class ResourceIdsWithReasonDto
    {
        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
}