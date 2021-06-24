using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Delivery;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Delivery : IIdEntity, ITrackCreation, ITrackUpdate, IHasDomainEvent
    {
        protected Delivery()
        {
        }

        public Delivery(int reference, Producer producer, DeliveryKind kind, DateTimeOffset scheduledOn,
            ExpectedAddress address,
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

            Reference = reference;
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
        public DateTimeOffset? RejectedOn { get; private set; }
        public int? Position { get; private set; }
        public string ReceptionedBy { get; private set; }
        public string Comment { get; private set; }
        public ExpectedAddress Address { get; private set; }
        public int PurchaseOrdersCount { get; private set; }
        public int ProductsToDeliverCount { get; private set; }
        public int ReturnablesCount { get; private set; }
        public int ReturnedProductsCount { get; private set; }
        public int ReturnedReturnablesCount { get; private set; }
        public string DeliveryFormUrl { get; private set; }
        public string DeliveryReceiptUrl { get; private set; }
        public Guid? DeliveryBatchId { get; private set; }
        public Guid ProducerId { get; private set; }
        public Guid ClientId { get; private set; }
        public virtual DeliveryBatch DeliveryBatch { get; private set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; private set; }
        public virtual ICollection<DeliveryProduct> Products { get; private set; }
        public virtual ICollection<DeliveryReturnable> ReturnedReturnables { get; private set; }
        public byte[] RowVersion { get; set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public int Reference { get; set; }

        public void SetReturnedReturnables(IEnumerable<KeyValuePair<Returnable, int>> returnables)
        {
            if (returnables == null || !returnables.Any())
            {
                ReturnedReturnables = new List<DeliveryReturnable>();
                return;
            }

            if (ReturnedReturnables == null)
                ReturnedReturnables = new List<DeliveryReturnable>();

            var newReturnableIds = returnables.Select(r => r.Key.Id);
            var existingReturnableIds = ReturnedReturnables.Select(r => r.ReturnableId);

            var returnablesToRemove = newReturnableIds.Except(existingReturnableIds);
            foreach (var returnableToRemove in returnablesToRemove)
            {
                var returnable = ReturnedReturnables.SingleOrDefault(r => r.ReturnableId == returnableToRemove);
                ReturnedReturnables.Remove(returnable);
            }

            foreach (var returnable in returnables)
            {
                var existingReturnable = ReturnedReturnables.SingleOrDefault(r => r.ReturnableId == returnable.Key.Id);
                if (existingReturnable != null)
                    existingReturnable.SetQuantity(returnable.Value);
                else
                    ReturnedReturnables.Add(new DeliveryReturnable(returnable.Key, returnable.Value));
            }

            Refresh();
        }

        public void AddPurchaseOrders(IEnumerable<PurchaseOrder> purchaseOrders)
        {
            if (DeliveryBatch == null && Status != DeliveryStatus.Waiting || DeliveryBatch != null &&
                DeliveryBatch.Status != DeliveryBatchStatus.Cancelled && Status != DeliveryStatus.Waiting)
                throw SheaftException.Validation();

            if (purchaseOrders == null || !purchaseOrders.Any())
                return;

            PurchaseOrders ??= new List<PurchaseOrder>();
            foreach (var purchaseOrder in purchaseOrders)
                PurchaseOrders.Add(purchaseOrder);

            Products ??= new List<DeliveryProduct>();
            var purchaseOrderProducts = purchaseOrders.SelectMany(p => p.Products).GroupBy(p => p.ProductId);
            foreach (var groupedPurchaseOrderProduct in purchaseOrderProducts)
            {
                var groupedProduct = groupedPurchaseOrderProduct.First();
                var quantity = groupedPurchaseOrderProduct.Sum(p => p.Quantity);
                var existingProduct = Products.FirstOrDefault(p =>
                    p.ProductId == groupedProduct.ProductId && p.RowKind == ModificationKind.Deliver);
                if (existingProduct != null)
                {
                    existingProduct.AddQuantity(quantity);
                    if (existingProduct.Quantity <= 0)
                        Products.Remove(existingProduct);
                }
                else if (quantity > 0)
                    Products.Add(new DeliveryProduct(groupedProduct, quantity, ModificationKind.Deliver,
                        groupedProduct.PurchaseOrderId));
            }

            Refresh();
        }

        public void RemovePurchaseOrders(IEnumerable<PurchaseOrder> purchaseOrders)
        {
            if (DeliveryBatch == null && Status != DeliveryStatus.Waiting || DeliveryBatch != null &&
                DeliveryBatch.Status != DeliveryBatchStatus.Cancelled && Status != DeliveryStatus.Waiting)
                throw SheaftException.Validation();

            if (PurchaseOrders == null)
                throw SheaftException.NotFound();

            foreach (var purchaseOrder in purchaseOrders)
                PurchaseOrders.Remove(purchaseOrder);

            var purchaseOrderProducts = purchaseOrders.SelectMany(p => p.Products).GroupBy(p => p.ProductId);
            foreach (var groupedPurchaseOrderProduct in purchaseOrderProducts)
            {
                var groupedProduct = groupedPurchaseOrderProduct.First();
                var existingProduct = Products.FirstOrDefault(p =>
                    p.ProductId == groupedProduct.ProductId && p.RowKind == ModificationKind.Deliver);
                if (existingProduct == null)
                    continue;

                var quantity = groupedPurchaseOrderProduct.Sum(p => p.Quantity);
                existingProduct.RemoveQuantity(quantity);
                if (existingProduct.Quantity <= 0)
                    Products.Remove(existingProduct);
            }

            Refresh();
        }

        public void SetAsReady()
        {
            if (Status != DeliveryStatus.Waiting)
                throw SheaftException.Validation();

            StartedOn = null;
            DeliveredOn = null;
            DeliveryFormUrl = null;
            DeliveryReceiptUrl = null;

            Status = DeliveryStatus.Ready;
            DomainEvents.Add(new DeliveryReadyEvent(Id));
        }

        public void StartDelivery()
        {
            if (Status != DeliveryStatus.Ready && Status != DeliveryStatus.Waiting)
                throw SheaftException.Validation();

            if (DeliveryBatch != null && DeliveryBatch.Status != DeliveryBatchStatus.InProgress)
                throw SheaftException.Validation();

            StartedOn = DateTimeOffset.UtcNow;
            DeliveredOn = null;
            Status = DeliveryStatus.InProgress;

            foreach (var purchaseOrder in PurchaseOrders)
                purchaseOrder.SetStatus(PurchaseOrderStatus.Shipping, true);

            DomainEvents.Add(new DeliveryStartedEvent(Id));
        }

        public void RejectDelivery(string comment)
        {
            if (Status != DeliveryStatus.InProgress)
                throw SheaftException.Validation();

            if (DeliveryBatch != null && DeliveryBatch.Status != DeliveryBatchStatus.InProgress)
                throw SheaftException.Validation();

            Comment = comment;
            RejectedOn = DateTimeOffset.UtcNow;
            Status = DeliveryStatus.Rejected;
            DomainEvents.Add(new DeliveryRejectedEvent(Id));
        }

        public void CompleteDelivery(IEnumerable<Tuple<DeliveryProduct, int, ModificationKind>> returnedProducts = null,
            IEnumerable<KeyValuePair<Returnable, int>> returnedReturnables = null, string receptionedBy = null,
            string comment = null)
        {
            if (Status is DeliveryStatus.Delivered)
                throw SheaftException.Validation();

            ReceptionedBy = receptionedBy;
            Comment = comment;
            StartedOn ??= DateTimeOffset.UtcNow;
            DeliveredOn = DateTimeOffset.UtcNow;
            Status = DeliveryStatus.Delivered;

            if (returnedProducts != null && returnedProducts.Any())
            {
                foreach (var productReturned in returnedProducts)
                {
                    var product = Products.FirstOrDefault(p => p.ProductId == productReturned.Item1.ProductId);
                    if (product == null)
                        throw SheaftException.NotFound();

                    Products.Add(new DeliveryProduct(product, productReturned.Item2, productReturned.Item3));
                }
            }

            if (returnedReturnables != null && returnedReturnables.Any())
                SetReturnedReturnables(returnedReturnables);

            foreach (var purchaseOrder in PurchaseOrders)
                purchaseOrder.SetStatus(PurchaseOrderStatus.Delivered, true);

            DomainEvents.Add(new DeliveryCompletedEvent(Id));
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

            Status = DeliveryStatus.Waiting;
            DomainEvents.Add(new DeliveryPostponedEvent(Id));
        }

        private void Refresh()
        {
            ReturnedReturnablesCount = ReturnedReturnables?.Sum(p => p.Quantity) ?? 0;
            ReturnedProductsCount = Products.Where(p => p.RowKind != ModificationKind.Deliver).Sum(p => p.Quantity);
            ReturnablesCount = Products.Where(p => p.RowKind == ModificationKind.Deliver && p.HasReturnable)
                .Sum(p => p.Quantity);
            ProductsToDeliverCount = Products.Where(p => p.RowKind == ModificationKind.Deliver).Sum(p => p.Quantity);
            PurchaseOrdersCount = PurchaseOrders.Count;

            DeliveryBatch?.Refresh();
        }

        public void SetFormUrl(string url)
        {
            DeliveryFormUrl = url;
            DomainEvents.Add(new DeliveryFormGeneratedEvent(Id));
        }

        public void SetReceiptUrl(string url)
        {
            DeliveryReceiptUrl = url;
            DomainEvents.Add(new DeliveryReceiptGeneratedEvent(Id));
        }
    }
}