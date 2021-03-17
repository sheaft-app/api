using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class SearchProducersDeliveriesInput
    {
        public IEnumerable<Guid> Ids { get; set; }
        public IEnumerable<DeliveryKind> Kinds { get; set; }
    }
}