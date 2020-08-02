using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class SearchProducersDeliveriesInput
    {
        public IEnumerable<Guid> Ids { get; set; }
        public IEnumerable<DeliveryKind> Kinds { get; set; }
    }
}