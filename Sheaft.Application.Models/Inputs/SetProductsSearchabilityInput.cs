using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class SetProductsSearchabilityInput
    {
        public IEnumerable<Guid> Ids { get; set; }
        public bool Searchable { get; set; }
    }
}