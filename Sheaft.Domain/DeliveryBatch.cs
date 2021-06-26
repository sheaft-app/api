﻿using System;
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
            IEnumerable<Delivery> deliveries = null)
        {
            Id = id;
            Name = name;
            ScheduledOn = scheduledOn;
            Day = scheduledOn.DayOfWeek;
            From = from;
            AssignedTo = assignedTo;
            AssignedToId = assignedTo.Id;
            Status = DeliveryBatchStatus.Waiting;

            SetDeliveries(deliveries);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Name { get; set; }
        public DeliveryBatchStatus Status { get; private set; }
        public int ProductsToDeliverCount { get; private set; }
        public int DeliveriesCount { get; private set; }
        public int ReturnablesCount { get; private set; }
        public int ReturnedReturnablesCount { get; private set; }
        public int ReturnedProductsCount { get; private set; }
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
        }

        public void CompleteBatch(bool partial = false)
        {
            if (Deliveries.Any(d => d.Status != DeliveryStatus.Delivered && d.Status != DeliveryStatus.Rejected))
                throw SheaftException.Validation();

            CompletedOn = DateTimeOffset.UtcNow;
            Status = partial ? DeliveryBatchStatus.Partial : DeliveryBatchStatus.Completed;
        }

        public void CancelBatch(string reason)
        {
            if (Status is DeliveryBatchStatus.Completed or DeliveryBatchStatus.Cancelled)
                throw SheaftException.Validation();

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
                throw SheaftException.Validation();

            if (Deliveries.Any(d => d.Status == DeliveryStatus.Delivered || d.Status == DeliveryStatus.Rejected))
                throw SheaftException.Validation();

            Status = Status == DeliveryBatchStatus.InProgress ? DeliveryBatchStatus.Ready : Status;
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
            ReturnedProductsCount = Deliveries.Sum(d => d.ReturnedProductsCount);
            ReturnedReturnablesCount = Deliveries.Sum(d => d.ReturnedReturnablesCount);
        }

        public void RemoveDelivery(Delivery delivery)
        {
            if (Deliveries == null)
                throw SheaftException.NotFound();

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