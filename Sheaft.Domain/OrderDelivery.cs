using System;

namespace Sheaft.Domain
{
    public class OrderDelivery
    {
        protected OrderDelivery()
        {
        }

        public OrderDelivery(DeliveryMode delivery, DateTimeOffset expectedDeliveryDate, string comment = null)
        {
            Id = delivery.Id;
            DeliveryMode = delivery;
            Comment = comment;
            ExpectedDelivery = new ExpectedOrderDelivery(delivery, expectedDeliveryDate);
        }

        public Guid Id { get; private set; }
        public string Comment { get; private set; }
        public virtual ExpectedOrderDelivery ExpectedDelivery { get; private set; }
        public virtual DeliveryMode DeliveryMode { get; private set; }
    }
}