using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PurchaseOrderDelivery : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected PurchaseOrderDelivery()
        {
        }

        public PurchaseOrderDelivery(OrderDelivery delivery, ExpectedAddress address, string client)
        {
            Id = Guid.NewGuid();
            Name = delivery.DeliveryMode.Name;
            Kind = delivery.DeliveryMode.Kind;
            Status = DeliveryStatus.Waiting;
            Client = client;

            ExpectedDeliveryDate = delivery.ExpectedDeliveryDate;
            From = delivery.From;
            To = delivery.To;
            Day = delivery.Day;

            Address = address;

            DeliveryModeId = delivery.DeliveryMode.Id;
            DeliveryMode = delivery.DeliveryMode;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string Name { get; private set; }
        public string Client { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DeliveryStatus Status { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? DeliveredOn { get; private set; }
        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
        public int? Position { get; private set; }
        public string ReceptionedBy { get; private set; }
        public string Comment { get; private set; }
        public ExpectedAddress Address { get; private set; }
        public Guid DeliveryModeId { get; private set; }
        public Guid PurchaseOrderId { get; private set; }
        public Guid? DeliveryBatchId { get; private set; }
        public virtual DeliveryMode DeliveryMode { get; private set; }
        public virtual PurchaseOrder PurchaseOrder { get; private set; }
        public virtual DeliveryBatch DeliveryBatch { get; private set; }

        public void SetAsReady()
        {
            if (Status != DeliveryStatus.Waiting)
                throw SheaftException.Validation();
            
            Status = DeliveryStatus.Ready;
        }

        public void StartDelivery()
        {
            if (Status != DeliveryStatus.Ready && Status != DeliveryStatus.Waiting)
                throw SheaftException.Validation();
            
            StartedOn = DateTimeOffset.UtcNow;
            Status = DeliveryStatus.InProgress;
            PurchaseOrder.SetStatus(PurchaseOrderStatus.Shipping, true);
        }

        public void CompleteDelivery(string receptionedBy, string comment)
        {
            if (Status is DeliveryStatus.Delivered)
                throw SheaftException.Validation();
            
            ReceptionedBy = receptionedBy;
            Comment = comment;
            StartedOn ??= DateTimeOffset.UtcNow;
            DeliveredOn = DateTimeOffset.UtcNow;
            Status = DeliveryStatus.Delivered;
            PurchaseOrder.SetStatus(PurchaseOrderStatus.Delivered, true);
        }

        public void SetPosition(int position)
        {
            Position = position;
        }

        public void SkipDelivery()
        {
            if (Status is DeliveryStatus.Delivered)
                throw SheaftException.Validation();
            
            Status = DeliveryStatus.Skipped;
        }

        public void PostponeDelivery()
        {
            if (Status is DeliveryStatus.Delivered)
                throw SheaftException.Validation();
            
            Status = DeliveryStatus.Postponed;
        }
    }
}