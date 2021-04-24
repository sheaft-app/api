using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;

namespace Sheaft.Domain
{

    public class ExpectedOrderDelivery : ExpectedDelivery
    {
        protected ExpectedOrderDelivery()
        {
        }

        public ExpectedOrderDelivery(DeliveryMode mode, DateTimeOffset expectedDeliveryDate)
            : base(mode, expectedDeliveryDate)
        {
            if (expectedDeliveryDate < DateTimeOffset.UtcNow)
                throw new ValidationException(MessageKind.ExpectedDelivery_ExpectedDate_CannotBe_BeforeNow, expectedDeliveryDate.ToString("dd/MM/yyyy"));

        }
    }
}