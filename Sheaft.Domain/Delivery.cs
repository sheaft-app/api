using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Delivery : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected Delivery()
        {
        }

        public Delivery(Producer producer, DeliveryKind kind, DateTimeOffset scheduledOn, ExpectedAddress address,
            Guid clientId, string clientName, IEnumerable<PurchaseOrder> purchaseOrders, int? position)
        {
            Id = Guid.NewGuid();
            Status = DeliveryStatus.Waiting;
            Client = clientName;
            ClientId = clientId;

            Kind = kind;
            ScheduledOn = scheduledOn;
            Position = position;
            Address = address;
            ProducerId = producer.Id;

            AddPurchaseOrders(purchaseOrders);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string Client { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DeliveryStatus Status { get; private set; }
        public DateTimeOffset ScheduledOn { get; private set; }
        public DateTimeOffset? StartedOn { get; private set; }
        public DateTimeOffset? DeliveredOn { get; private set; }
        public int? Position { get; private set; }
        public string ReceptionedBy { get; private set; }
        public string Comment { get; private set; }
        public ExpectedAddress Address { get; private set; }
        public int PurchaseOrdersCount { get; private set; }
        public Guid? DeliveryBatchId { get; private set; }
        public Guid ProducerId { get; private set; }
        public Guid ClientId { get; private set; }
        public virtual DeliveryBatch DeliveryBatch { get; private set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; private set; }

        public void AddPurchaseOrders(IEnumerable<PurchaseOrder> purchaseOrders)
        {
            if (purchaseOrders == null || !purchaseOrders.Any())
                return;

            if (PurchaseOrders == null)
                PurchaseOrders = new List<PurchaseOrder>();

            foreach (var purchaseOrder in purchaseOrders)
                PurchaseOrders.Add(purchaseOrder);

            Refresh();
        }

        public void RemovePurchaseOrders(IEnumerable<PurchaseOrder> purchaseOrders)
        {
            if (PurchaseOrders == null)
                throw SheaftException.NotFound();

            foreach (var purchaseOrder in purchaseOrders)
                PurchaseOrders.Remove(purchaseOrder);

            Refresh();
        }

        public void SetAsReady()
        {
            if (Status != DeliveryStatus.Waiting && Status != DeliveryStatus.Postponed)
                throw SheaftException.Validation();

            StartedOn = null;
            DeliveredOn = null;

            Status = DeliveryStatus.Ready;
        }

        public void StartDelivery()
        {
            if (Status != DeliveryStatus.Ready && Status != DeliveryStatus.Waiting && Status != DeliveryStatus.Postponed)
                throw SheaftException.Validation();

            if(DeliveryBatch != null && DeliveryBatch.Status != DeliveryBatchStatus.InProgress)
                throw SheaftException.Validation();

            StartedOn = DateTimeOffset.UtcNow;
            DeliveredOn = null;
            Status = DeliveryStatus.InProgress;

            foreach (var purchaseOrder in PurchaseOrders)
                purchaseOrder.SetStatus(PurchaseOrderStatus.Shipping, true);
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

            foreach (var purchaseOrder in PurchaseOrders)
                purchaseOrder.SetStatus(PurchaseOrderStatus.Delivered, true);
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

            foreach (var purchaseOrder in PurchaseOrders)
                purchaseOrder.SetStatus(PurchaseOrderStatus.Postponed, true);
        }

        private void Refresh()
        {
            PurchaseOrdersCount = PurchaseOrders.Count;
            DeliveryBatch?.Refresh();
        }
    }
}