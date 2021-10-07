using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.DeliveryBatch;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class DeliveryBatch : IIdEntity, ITrackCreation, ITrackUpdate, ITrackRemove, IHasDomainEvent
    {
        protected DeliveryBatch()
        {
        }

        public DeliveryBatch(Guid id, string name, DateTimeOffset scheduledOn, TimeSpan from, User assignedTo,
            IEnumerable<Delivery> deliveries = null, Guid? createdFromPartialBatchId = null)
        {
            Id = id;
            Name = name;
            ScheduledOn = scheduledOn;
            Day = scheduledOn.DayOfWeek;
            From = from;
            AssignedTo = assignedTo;
            AssignedToId = assignedTo.Id;
            Status = DeliveryBatchStatus.Waiting;
            CreatedFromBatchId = createdFromPartialBatchId;

            SetDeliveries(deliveries);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; set; }
        public DeliveryBatchStatus Status { get; private set; }
        public int ProductsToDeliverCount { get; private set; }
        public int ProductsDeliveredCount { get; private set; }
        public int DeliveriesCount { get; private set; }
        public int ReturnablesCount { get; private set; }
        public int ReturnedReturnablesCount { get; private set; }
        public int BrokenProductsCount { get; private set; }
        public int MissingProductsCount { get; private set; }
        public int ImproperProductsCount { get; private set; }
        public int ExcessProductsCount { get; private set; }
        public int PurchaseOrdersCount { get; private set; }
        public DateTimeOffset ScheduledOn { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan From { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public DateTimeOffset? CancelledOn { get; private set; }
        public string Reason { get; private set; }
        public string DeliveryFormsUrl { get; private set; }
        public Guid AssignedToId { get; private set; }
        public Guid? CreatedFromBatchId { get; set; }
        public virtual User AssignedTo { get; private set; }
        public virtual ICollection<Delivery> Deliveries { get; private set; }
        public byte[] RowVersion { get; set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            Name = name;
        }

        public void SetBatchReady()
        {
            Reason = null;
            StartedOn = null;
            Status = DeliveryBatchStatus.Ready;

            foreach (var delivery in Deliveries)
                delivery.SetAsReady();
        }

        public void StartBatch()
        {
            Reason = null;
            StartedOn = DateTimeOffset.UtcNow;
            Status = DeliveryBatchStatus.InProgress;

            foreach (var delivery in Deliveries.Where(d => d.Status is DeliveryStatus.Waiting or DeliveryStatus.Skipped or DeliveryStatus.InProgress))
                delivery.SetAsReady();
        }

        public void CompleteBatch(bool partial = false)
        {
            if (Deliveries.Any(d => d.Status != DeliveryStatus.Delivered && d.Status != DeliveryStatus.Rejected))
                throw SheaftException.Validation("La tournée contient des livraisons en cours.");

            CompletedOn = DateTimeOffset.UtcNow;
            Status = partial ? DeliveryBatchStatus.Partial : DeliveryBatchStatus.Completed;
        }

        public void CancelBatch(string reason)
        {
            if (Status is DeliveryBatchStatus.Completed)
                throw SheaftException.Validation("La tournée est déjà terminée.");
                
            if (Status is DeliveryBatchStatus.Cancelled)
                throw SheaftException.Validation("La tournée est déjà annulée.");

            CancelledOn = DateTimeOffset.UtcNow;
            Reason = reason;
            Status = DeliveryBatchStatus.Cancelled;
            
            foreach (var delivery in Deliveries.Where(d => d.Status != DeliveryStatus.Delivered && d.Status != DeliveryStatus.Rejected).ToList())
            {
                var purchaseOrders = delivery.PurchaseOrders.Where(po => po.Status != PurchaseOrderStatus.Delivered);
                delivery.RemovePurchaseOrders(purchaseOrders);
            }
        }

        public void PostponeBatch(DateTimeOffset rescheduledOn, TimeSpan from, string reason)
        {
            if (Status != DeliveryBatchStatus.Waiting && Status != DeliveryBatchStatus.Ready && Status != DeliveryBatchStatus.InProgress)
                throw SheaftException.Validation("La tournée doit être en attente, prête ou en cours pour la décaler.");

            if (Deliveries.Any(d => d.Status == DeliveryStatus.Delivered || d.Status == DeliveryStatus.Rejected))
                throw SheaftException.Validation("La tournée contient déjà des livraisons terminées ou refusées, veuillez compléter la tournée en précisant une nouvelle date.");

            StartedOn = null;
            ScheduledOn = rescheduledOn;
            From = from;
            Reason = reason;
            
            foreach (var delivery in Deliveries)
                delivery.PostponeDelivery();
            
            DomainEvents.Add(new DeliveryBatchPostponedEvent(Id));
        }

        public void AddDelivery(Delivery delivery)
        {
            if (Deliveries == null)
                Deliveries = new List<Delivery>();

            Deliveries.Add(delivery);
            Refresh();
        }

        private void SetDeliveries(IEnumerable<Delivery> deliveries)
        {
            if (Deliveries == null || Deliveries.Any())
                Deliveries = new List<Delivery>();

            var positionCounter = 0;
            foreach (var delivery in deliveries.OrderBy(d => d.Position))
            {
                if(delivery.DeliveryBatchId.HasValue && delivery.DeliveryBatchId != Id)
                    delivery.DeliveryBatch.RemoveDelivery(delivery);
                
                delivery.SetPosition(positionCounter);
                Deliveries.Add(delivery);
                positionCounter++;
            }

            Refresh();
        }

        internal void Refresh()
        {
            DeliveriesCount = Deliveries.Count;
            ReturnablesCount = Deliveries.Sum(d => d.ReturnablesCount);
            ProductsToDeliverCount = Deliveries.Sum(d => d.ProductsToDeliverCount);
            PurchaseOrdersCount = Deliveries.Sum(d => d.PurchaseOrdersCount);
            BrokenProductsCount = Deliveries.Sum(d => d.BrokenProductsCount);
            MissingProductsCount = Deliveries.Sum(d => d.MissingProductsCount);
            ImproperProductsCount = Deliveries.Sum(d => d.ImproperProductsCount);
            ExcessProductsCount = Deliveries.Sum(d => d.ExcessProductsCount);
            ReturnedReturnablesCount = Deliveries.Sum(d => d.ReturnedReturnablesCount);
            ProductsDeliveredCount = Deliveries.Sum(d => d.ProductsDeliveredCount);
        }

        public void RemoveDelivery(Delivery delivery)
        {
            if (Deliveries == null)
                throw SheaftException.NotFound("Cette tournée ne contient pas de livraisons.");

            Deliveries.Remove(delivery);
            Refresh();

            if (DeliveriesCount < 1)
                Status = DeliveryBatchStatus.Cancelled;
        }

        public void SetDeliveryFormsUrl(string url)
        {
            DeliveryFormsUrl = url;
            DomainEvents.Add(new DeliveryBatchFormsGeneratedEvent(Id));
        }
    }
}