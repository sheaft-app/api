using System;

namespace Sheaft.Domain.Models
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
            ExpectedDeliveryDate = expectedDeliveryDate;
            Comment = comment;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public virtual DeliveryMode DeliveryMode { get; private set; }
        public string Comment { get; private set; }
    }
}