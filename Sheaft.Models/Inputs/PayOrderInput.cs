using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{

    public class PayOrderInput
    {
        public decimal Donation { get; set; }
        public Guid OrderId { get; set; }
    }
}