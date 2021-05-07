using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class OrderDelivery : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected OrderDelivery()
        {
        }

        public OrderDelivery(DeliveryMode delivery, DateTimeOffset expectedDeliveryDate, string comment = null)
        {
            Id = Guid.NewGuid();
            DeliveryMode = delivery;
            DeliveryModeId = delivery.Id;
            Comment = comment;
            ExpectedDelivery = new ExpectedOrderDelivery(delivery, expectedDeliveryDate);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string Comment { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid DeliveryModeId { get; private set; }
        public ExpectedOrderDelivery ExpectedDelivery { get; private set; }
        public virtual DeliveryMode DeliveryMode { get; private set; }
        public byte[] RowVersion { get; private set; }
    }
}