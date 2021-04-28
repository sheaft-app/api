using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class OrderDelivery : IIdEntity
    {
        protected OrderDelivery()
        {
        }

        public OrderDelivery(DeliveryMode delivery, DateTimeOffset expectedDeliveryDate, string comment = null)
        {
            Id = delivery.Id;
            DeliveryMode = delivery;
            DeliveryModeId = delivery.Id;
            Comment = comment;
            ExpectedDelivery = new ExpectedOrderDelivery(delivery, expectedDeliveryDate);
        }

        public Guid Id { get; private set; }
        public string Comment { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid DeliveryModeId { get; private set; }
        public virtual ExpectedOrderDelivery ExpectedDelivery { get; private set; }
        public virtual DeliveryMode DeliveryMode { get; private set; }
    }
}