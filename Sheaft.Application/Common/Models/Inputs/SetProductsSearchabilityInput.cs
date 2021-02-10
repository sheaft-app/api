using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class SetProductsSearchabilityInput
    {
        public IEnumerable<Guid> Ids { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
    }
}