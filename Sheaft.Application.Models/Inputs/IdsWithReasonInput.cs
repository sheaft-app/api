using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class IdsWithReasonInput
    {
        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
}