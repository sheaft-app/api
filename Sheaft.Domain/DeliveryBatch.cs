using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class DeliveryBatch : IIdEntity, ITrackCreation, ITrackUpdate, ITrackRemove
    {
        protected DeliveryBatch()
        {
        }

        public DeliveryBatch(Guid id, string name, DateTimeOffset scheduledOn, TimeSpan from, User assignedTo, List<PurchaseOrderDelivery> purchaseOrderDeliveries = null)
        {
            Id = id;
            Name = name;
            ScheduledOn = scheduledOn;
            Day = scheduledOn.DayOfWeek;
            From = from;
            AssignedTo = assignedTo;
            AssignedToId = assignedTo.Id;
            Status = DeliveryBatchStatus.Waiting;
            
            SetDeliveries(purchaseOrderDeliveries);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; set; }
        public DeliveryBatchStatus Status { get; private set; }
        public int ProductsCount { get; private set; }
        public int DeliveriesCount { get; private set; }
        public DateTimeOffset ScheduledOn { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public DateTimeOffset? CancelledOn { get; private set; }
        public string Reason { get; private set; }
        public Guid AssignedToId { get; private set; }
        public virtual User AssignedTo { get; private set; }
        public virtual ICollection<PurchaseOrderDelivery> Deliveries { get; private set; }
        public byte[] RowVersion { get; set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            Name = name;
        }
        
        public void StartBatch()
        {
            Reason = null;
            StartedOn ??= DateTimeOffset.UtcNow;
            Status = DeliveryBatchStatus.InProgress;
            
            foreach (var delivery in Deliveries)
                delivery.SetAsReady();
        }

        public void CompleteBatch()
        {
            if(Deliveries.Any(d => d.Status != DeliveryStatus.Delivered))
                throw SheaftException.Validation();
            
            CompletedOn = DateTimeOffset.UtcNow;
            Status = DeliveryBatchStatus.Completed;
        }

        public void CancelBatch(string reason)
        {
            if(Status is DeliveryBatchStatus.Completed or DeliveryBatchStatus.Cancelled)
                throw SheaftException.Validation();

            foreach (var delivery in Deliveries.Where(d => d.Status != DeliveryStatus.Delivered).ToList())
                Deliveries.Remove(delivery);
            
            CancelledOn = DateTimeOffset.UtcNow;
            Reason = reason;
            Status = DeliveryBatchStatus.Cancelled;
        }

        public void PostponeBatch(DateTimeOffset rescheduledOn, TimeSpan from, string reason)
        {
            if(Status != DeliveryBatchStatus.Waiting)
                throw SheaftException.Validation();

            StartedOn = null;
            ScheduledOn = rescheduledOn;
            From = from;
            Reason = reason;
            Status = DeliveryBatchStatus.Waiting;
            foreach (var delivery in Deliveries)
                delivery.PostponeDelivery();
        }

        public void SetDeliveries(IEnumerable<PurchaseOrderDelivery> deliveries)
        {
            if (Deliveries == null || Deliveries.Any())
                Deliveries = new List<PurchaseOrderDelivery>();

            var positionCounter = 0;
            foreach (var delivery in deliveries.OrderBy(d => d.Position))
            {
                delivery.SetPosition(positionCounter);
                Deliveries.Add(delivery);
                positionCounter++;
                
                if(delivery.DeliveryBatchId.HasValue && delivery.DeliveryBatchId != Id)
                    delivery.PostponeDelivery();
            }

            DeliveriesCount = Deliveries.Count;
            ProductsCount = Deliveries.Sum(d => d.PurchaseOrder.ProductsCount);
        }
    }
}