using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class DeliveryBatch : IIdEntity, ITrackCreation, ITrackUpdate, ITrackRemove
    {
        protected DeliveryBatch()
        {
        }

        public DeliveryBatch(Guid id, User assignedTo)
        {
            Id = id;
            AssignedTo = assignedTo;
            Status = DeliveryBatchStatus.Waiting;
        }

        public Guid Id { get; private set; }

        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DeliveryBatchStatus Status { get; private set; }
        public int ProductsCount { get; private set; }
        public int DeliveriesCount { get; private set; }
        public DateTimeOffset ScheduledOn { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public User AssignedTo { get; private set; }
        public virtual ICollection<PurchaseOrderDelivery> Deliveries { get; private set; }

        public void StartBatch()
        {
            StartedOn ??= DateTimeOffset.UtcNow;
            Status = DeliveryBatchStatus.InProgress;
            
            foreach (var delivery in Deliveries.Where(d => d.Status != DeliveryStatus.Delivered))
                delivery.StartDelivery();
        }

        public void CompleteBatch()
        {
            if(Deliveries.Any(d => d.Status != DeliveryStatus.Delivered))
                throw SheaftException.Validation();
            
            CompletedOn = DateTimeOffset.UtcNow;
            Status = DeliveryBatchStatus.Completed;
        }

        public void PostponeBatch(DateTimeOffset rescheduledOn, TimeSpan from, TimeSpan to)
        {
            if(Status != DeliveryBatchStatus.Waiting && Status != DeliveryBatchStatus.Postponed)
                throw SheaftException.Validation();
            
            ScheduledOn = rescheduledOn;
            From = from;
            To = to;
            
            Status = DeliveryBatchStatus.Postponed;
            foreach (var delivery in Deliveries.Where(d => d.Status != DeliveryStatus.Delivered))
                delivery.PostponeDelivery();
        }

        public void SetDeliveries(IEnumerable<PurchaseOrderDelivery> deliveries, bool postponed = false)
        {
            if (Deliveries == null || Deliveries.Any())
                Deliveries = new List<PurchaseOrderDelivery>();

            var positionCounter = 0;
            foreach (var delivery in deliveries.OrderBy(d => d.Position))
            {
                delivery.SetPosition(positionCounter);
                if(!postponed)
                    delivery.SetAsReady();
                else
                    delivery.PostponeDelivery();
                
                Deliveries.Add(delivery);

                positionCounter++;
            }

            DeliveriesCount = Deliveries.Count;
            ProductsCount = Deliveries.Sum(d => d.PurchaseOrder.ProductsCount);
        }
    }
}