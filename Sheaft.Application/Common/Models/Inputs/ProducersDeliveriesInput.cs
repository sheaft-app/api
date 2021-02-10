using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class SearchProducersDeliveriesInput
    {
        public IEnumerable<Guid> Ids { get; set; }
        public IEnumerable<DeliveryKind> Kinds { get; set; }
    }
}