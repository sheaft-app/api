using System;
using System.Collections.Generic;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class CreateDeliveryClosingsInput
    {
        public Guid DeliveryId { get; set; }
        public List<ClosingInput> Closings { get; set; }
    }
}