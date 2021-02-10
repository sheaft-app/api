using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class SetDeliveryModesAvailabilityInput
    {
        public IEnumerable<Guid> Ids { get; set; }
        public bool Available { get; set; }
    }
}