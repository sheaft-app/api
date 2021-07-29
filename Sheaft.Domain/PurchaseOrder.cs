using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PurchaseOrder : IEntity, IHasDomainEvent
    {
        private const int DIGITS_COUNT = 2;

        protected PurchaseOrder()
        {
        }

        public PurchaseOrder(Guid id, int reference, PurchaseOrderStatus status, Producer producer, Order order)
        {
            if (producer == null)
                throw SheaftException.Validation("Impossible de créer la commande, le producteur est requis.");

            Id = id;

            Products = new List<PurchaseOrderProduct>();
            DomainEvents = new List<DomainEvent> {new PurchaseOrderCreatedEvent(Id)};

            SetSender(order.User);
            SetVendor(producer);

            var delivery = order.Deliveries.Single(d => d.DeliveryMode.ProducerId == producer.Id);
            var address = (int) delivery.DeliveryMode.Kind <= 4
                ? new ExpectedAddress(delivery.DeliveryMode.Address.Line1, delivery.DeliveryMode.Address.Line2,
                    delivery.DeliveryMode.Address.Zipcode, delivery.DeliveryMode.Address.City,
                    delivery.DeliveryMode.Address.Country, delivery.DeliveryMode.Address.Longitude,
                    delivery.DeliveryMode.Address.Latitude)
                : new ExpectedAddress(order.User.Address.Line1, order.User.Address.Line2, order.User.Address.Zipcode,
                    order.User.Address.City, order.User.Address.Country, order.User.Address.Longitude,
                    order.User.Address.Latitude);

            ExpectedDelivery = new ExpectedPurchaseOrderDelivery(delivery, address);
            
            SetComment(delivery.Comment);

            SetReference(reference);
            SetStatus(status, true);

            var orderProducts = order.Products.Where(p => p.ProducerId == producer.Id);
            foreach (var orderProduct in orderProducts)
                AddProduct(orderProduct);
        }
        public PurchaseOrder(Guid id, int reference, PurchaseOrderStatus status, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, DeliveryMode deliveryMode, Producer producer, User client, IEnumerable<KeyValuePair<Domain.Product, int>> products, Catalog catalog, bool skipNotification, string comment = null)
        {
            if (producer == null)
                throw SheaftException.Validation("Impossible de créer la commande, le producteur est requis.");

            Id = id;
            Products = new List<PurchaseOrderProduct>();
            DomainEvents = new List<DomainEvent> {new PurchaseOrderCreatedEvent(Id)};
            Status = PurchaseOrderStatus.Waiting;
            CreatedOn = DateTimeOffset.UtcNow;
            
            SetSender(client);
            SetVendor(producer);

            var address = (int) deliveryMode.Kind <= 4
                ? new ExpectedAddress(deliveryMode.Address.Line1, deliveryMode.Address.Line2,
                    deliveryMode.Address.Zipcode, deliveryMode.Address.City,
                    deliveryMode.Address.Country, deliveryMode.Address.Longitude,
                    deliveryMode.Address.Latitude)
                : new ExpectedAddress(client.Address.Line1, client.Address.Line2, client.Address.Zipcode,
                    client.Address.City, client.Address.Country, client.Address.Longitude,
                    client.Address.Latitude);

            ExpectedDelivery = new ExpectedPurchaseOrderDelivery(deliveryMode, expectedDeliveryDate, from, to, address);
            
            SetComment(comment);
            SetReference(reference);

            foreach (var product in products)
                AddProduct(new PurchaseOrderProduct(product.Key, catalog.Id, product.Value));
            
            SetStatus(status, skipNotification);
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? AcceptedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public DateTimeOffset? DroppedOn { get; private set; }
        public int Reference { get; private set; }
        public string Reason { get; private set; }
        public string Comment { get; private set; }
        public int LinesCount { get; private set; }
        public int ProductsCount { get; private set; }
        public int ReturnablesCount { get; private set; }
        public decimal TotalProductWholeSalePrice { get; private set; }
        public decimal TotalProductVatPrice { get; private set; }
        public decimal TotalProductOnSalePrice { get; private set; }
        public decimal TotalReturnableOnSalePrice { get; private set; }
        public decimal TotalReturnableWholeSalePrice { get; private set; }
        public decimal TotalReturnableVatPrice { get; private set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public decimal TotalWeight { get; private set; }
        public PurchaseOrderStatus Status { get; private set; }
        public Guid? OrderId { get; private set; }
        public Guid ProducerId { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid? DeliveryId { get; private set; }
        public Guid? PickingId { get; private set; }
        public PurchaseOrderSender SenderInfo { get; private set; }
        public PurchaseOrderVendor VendorInfo { get; private set; }
        public virtual Delivery Delivery { get; private set; }
        public virtual Picking Picking { get; private set; }
        public virtual ExpectedPurchaseOrderDelivery ExpectedDelivery { get; private set; }
        public virtual ICollection<PurchaseOrderProduct> Products { get; private set; }

        public void SetReference(int newReference)
        {
            Reference = newReference;
        }

        internal void SetStatus(PurchaseOrderStatus newStatus, bool skipNotification)
        {
            switch (newStatus)
            {
                case PurchaseOrderStatus.Accepted:
                    if (Status is PurchaseOrderStatus.Processing or PurchaseOrderStatus.Accepted)
                        break;
                    
                    if (Status != PurchaseOrderStatus.Waiting)
                        throw SheaftException.Validation("Impossible d'accepter la commande, elle n'est plus en attente.");

                    if (SenderInfo.Kind == ProfileKind.Consumer && CreatedOn.AddDays(3) < DateTimeOffset.UtcNow)
                        throw SheaftException.Validation("Impossible d'accepter la commande, le délai de 72h est écoulé.");

                    AcceptedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderAcceptedEvent(Id));

                    break;
                case PurchaseOrderStatus.Completed:
                    if (Status != PurchaseOrderStatus.Processing)
                        throw SheaftException.Validation("La commande doit être en préparation pour pouvoir être complétée.");

                    CompletedOn ??= DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderCompletedEvent(Id));
                    break;
                case PurchaseOrderStatus.Delivered:
                    if (Status != PurchaseOrderStatus.Completed)
                        throw SheaftException.Validation("La commande doit être en prête pour pouvoir être livrée.");
                    break;
                case PurchaseOrderStatus.Cancelled:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw SheaftException.Validation("La commande a déjà été annulée.");
                    if (Status == PurchaseOrderStatus.Refused)
                        throw SheaftException.Validation("La commande a déjà été refusée.");
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw SheaftException.Validation("La commande a déjà été livrée.");
                    if (Status == PurchaseOrderStatus.Expired)
                        throw SheaftException.Validation("La commande est expirée.");

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderCancelledEvent(Id));
                    break;
                case PurchaseOrderStatus.Withdrawned:
                    if (Status == PurchaseOrderStatus.Withdrawned)
                        throw SheaftException.Validation("La commande a déjà été annulée.");
                    if (Status == PurchaseOrderStatus.Refused)
                        throw SheaftException.Validation("La commande a déjà été refusée.");
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw SheaftException.Validation("La commande a déjà été livrée.");
                    if (Status == PurchaseOrderStatus.Expired)
                        throw SheaftException.Validation("La commande est expirée.");

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderWithdrawnedEvent(Id));
                    break;
                case PurchaseOrderStatus.Refused:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw SheaftException.Validation("La commande a déjà été annulée.");
                    if (Status == PurchaseOrderStatus.Refused)
                        throw SheaftException.Validation("La commande a déjà été refusée.");
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw SheaftException.Validation("La commande a déjà été livrée.");
                    if (Status == PurchaseOrderStatus.Expired)
                        throw SheaftException.Validation("La commande est expirée.");

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderRefusedEvent(Id));
                    break;
                case PurchaseOrderStatus.Expired:
                    if (Status != PurchaseOrderStatus.Waiting)
                        throw SheaftException.Validation("La commande ne peut pas être expirée, elle doit être en attente.");

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderExpiredEvent(Id));
                    break;
            }

            Status = newStatus;
        }

        public void SetComment(string comment)
        {
            Comment = comment;
        }

        public void Cancel(string reason, bool skipNotification)
        {
            if(PickingId.HasValue)
                Picking.RemovePurchaseOrders(new []{this});
            
            if(DeliveryId.HasValue)
                Delivery.RemovePurchaseOrders(new []{this});
            
            SetStatus(PurchaseOrderStatus.Cancelled, skipNotification);
            Reason = reason;
        }

        public void Withdrawn(string reason, bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Withdrawned, skipNotification);
            Reason = reason;
        }

        public void Expire(string reason, bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Expired, skipNotification);
            Reason = reason;
        }

        public void Refuse(string reason, bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Refused, skipNotification);
            Reason = reason;
        }

        public void Process(bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Processing, skipNotification);
        }

        public void Complete(bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Completed, skipNotification);
        }

        public void Accept(bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Accepted, skipNotification);
        }

        public void SetSender(User sender)
        {
            ClientId = sender.Id;
            SenderInfo = new PurchaseOrderSender(sender);
        }

        public void SetVendor(Producer vendor)
        {
            ProducerId = vendor.Id;
            VendorInfo = new PurchaseOrderVendor(vendor);
        }

        private void AddProduct(ProductRow product)
        {
            if (Status != PurchaseOrderStatus.Waiting)
                throw SheaftException.Validation("Impossible d'ajouter un produit, la commande n'est plus en attente.");

            if (product == null)
                throw SheaftException.Validation("Impossible d'ajouter le produit, il est introuvable.");

            var productLine = Products.SingleOrDefault(p => p.ProductId == product.ProductId);
            if (productLine != null)
                Products.Remove(productLine);

            Products.Add(new PurchaseOrderProduct(product));
            RefreshOrder();
        }

        private void RefreshOrder()
        {
            TotalProductWholeSalePrice = Math.Round(Products.Sum(p => p.TotalProductWholeSalePrice), DIGITS_COUNT);
            TotalProductVatPrice = Math.Round(Products.Sum(p => p.TotalProductVatPrice), DIGITS_COUNT);
            TotalProductOnSalePrice = Math.Round(TotalProductWholeSalePrice + TotalProductVatPrice, DIGITS_COUNT);

            TotalWeight = Math.Round(Products.Where(p => p.TotalWeight.HasValue).Sum(p => p.TotalWeight) ?? 0,
                DIGITS_COUNT);

            LinesCount = Products.Select(p => p.Id).Distinct().Count();
            ProductsCount = Products.Sum(p => p.Quantity);
            ReturnablesCount = Products.Where(p => p.HasReturnable).Sum(p => p.Quantity);

            TotalReturnableWholeSalePrice =
                Math.Round(Products.Sum(p => p.HasReturnable ? p.TotalReturnableWholeSalePrice.Value : 0),
                    DIGITS_COUNT);
            TotalReturnableVatPrice =
                Math.Round(Products.Sum(p => p.HasReturnable ? p.TotalReturnableVatPrice.Value : 0),
                    DIGITS_COUNT);
            TotalReturnableOnSalePrice =
                Math.Round(TotalReturnableWholeSalePrice + TotalReturnableVatPrice, DIGITS_COUNT);

            TotalWholeSalePrice = TotalProductWholeSalePrice + TotalReturnableWholeSalePrice;
            TotalVatPrice = TotalProductVatPrice + TotalReturnableVatPrice;
            TotalOnSalePrice = Math.Round(TotalWholeSalePrice + TotalVatPrice, DIGITS_COUNT);
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }
    }
}