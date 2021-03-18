using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Models
{
    public class SearchProducersDeliveriesDto
    {
        public IEnumerable<Guid> Ids { get; set; }
        public IEnumerable<DeliveryKind> Kinds { get; set; }
    }
}