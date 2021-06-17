using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PurchaseOrderDelivery : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected PurchaseOrderDelivery()
        {
        }

        public PurchaseOrderDelivery(OrderDelivery delivery)
        {
            Id = Guid.NewGuid();
            Name = delivery.DeliveryMode.Name;
            Kind = delivery.DeliveryMode.Kind;
            Status = DeliveryStatus.Waiting;

            ExpectedDeliveryDate = delivery.ExpectedDeliveryDate;
            From = delivery.From;
            To = delivery.To;
            Day = delivery.Day;

            Address = delivery.DeliveryMode.Address != null
                ? new ExpectedAddress(delivery.DeliveryMode.Address.Line1, delivery.DeliveryMode.Address.Line2,
                    delivery.DeliveryMode.Address.Zipcode, delivery.DeliveryMode.Address.City,
                    delivery.DeliveryMode.Address.Country, delivery.DeliveryMode.Address.Longitude,
                    delivery.DeliveryMode.Address.Latitude)
                : null;

            DeliveryModeId = delivery.DeliveryMode.Id;
            DeliveryMode = delivery.DeliveryMode;
        }

        public Guid Id { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string Name { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DeliveryStatus Status { get; private set; }
        public DateTimeOffset? DeliveredOn { get; private set; }
        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
        public string ReceptionedBy { get; private set; }
        public ExpectedAddress Address { get; private set; }
        public Guid DeliveryModeId { get; private set; }
        public Guid PurchaseOrderId { get; private set; }
        public virtual DeliveryMode DeliveryMode { get; private set; }

        public void SetAsReady()
        {
            Status = DeliveryStatus.Ready;
        }

        public void StartDelivery()
        {
            Status = DeliveryStatus.InProgress;
        }

        public void CompleteDelivery()
        {
            DeliveredOn = DateTimeOffset.UtcNow;
            Status = DeliveryStatus.Delivered;
        }

        public void CancelDelivery()
        {
            Status = DeliveryStatus.Cancelled;
        }

        public void PostponeDelivery()
        {
            Status = DeliveryStatus.Postponed;
        }
    }
}