using System;

namespace Sheaft.Domain.Models
{

    public class ExpectedOrderDelivery : ExpectedDelivery
    {
        protected ExpectedOrderDelivery()
        {
        }

        public ExpectedOrderDelivery(DeliveryMode mode, DateTimeOffset expectedDeliveryDate)
            : base(mode, expectedDeliveryDate)
        {
        }
    }
}