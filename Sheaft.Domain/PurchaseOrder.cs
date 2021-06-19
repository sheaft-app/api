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

        public PurchaseOrder(Guid id, string reference, PurchaseOrderStatus status, Producer producer, Order order)
        {
            if (producer == null)
                throw SheaftException.Validation(MessageKind.PurchaseOrder_Vendor_Required);

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

            Delivery = new PurchaseOrderDelivery(delivery, address, order.User.Name);
            SetComment(delivery.Comment);

            SetReference(reference);
            SetStatus(status, true);

            var orderProducts = order.Products.Where(p => p.ProducerId == producer.Id);
            foreach (var orderProduct in orderProducts)
                AddProduct(orderProduct);
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? AcceptedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public DateTimeOffset? DroppedOn { get; private set; }
        public string Reference { get; private set; }
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
        public Guid OrderId { get; private set; }
        public Guid ProducerId { get; private set; }
        public Guid ClientId { get; private set; }
        public PurchaseOrderSender SenderInfo { get; private set; }
        public PurchaseOrderVendor VendorInfo { get; private set; }
        public virtual PurchaseOrderDelivery Delivery { get; private set; }
        public virtual ICollection<PurchaseOrderProduct> Products { get; private set; }

        public void SetReference(string newReference)
        {
            if (string.IsNullOrWhiteSpace(newReference))
                throw SheaftException.Validation(MessageKind.PurchaseOrder_Reference_Required);

            Reference = newReference;
        }

        internal void SetStatus(PurchaseOrderStatus newStatus, bool skipNotification)
        {
            switch (newStatus)
            {
                case PurchaseOrderStatus.Accepted:
                    if (Status != PurchaseOrderStatus.Waiting)
                        throw SheaftException.Validation(MessageKind.PurchaseOrder_CannotAccept_NotIn_WaitingStatus);

                    if (SenderInfo.Kind == ProfileKind.Consumer && CreatedOn.AddDays(5) < DateTimeOffset.UtcNow)
                        throw SheaftException.Validation();

                    AcceptedOn = DateTimeOffset.UtcNow;
                    Status = PurchaseOrderStatus.Processing;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderAcceptedEvent(Id));

                    return;
                case PurchaseOrderStatus.Completed:
                    if (Status != PurchaseOrderStatus.Processing)
                        throw SheaftException.Validation(
                            MessageKind.PurchaseOrder_CannotComplete_NotIn_ProcessingStatus);

                    CompletedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderCompletedEvent(Id));

                    break;
                case PurchaseOrderStatus.Shipping:
                    if (Status != PurchaseOrderStatus.Completed)
                        throw SheaftException.Validation(MessageKind.PurchaseOrder_CannotShip_NotIn_CompletedStatus);

                    break;
                case PurchaseOrderStatus.Delivered:
                    if (Status != PurchaseOrderStatus.Completed && Status != PurchaseOrderStatus.Shipping)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotDeliver_NotIn_CompletedOrShippingStatus);
                    break;
                case PurchaseOrderStatus.Cancelled:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotCancel_AlreadyIn_CancelledStatus);
                    if (Status == PurchaseOrderStatus.Refused)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotCancel_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotCancel_AlreadyIn_DeliveredStatus);
                    if (Status == PurchaseOrderStatus.Expired)
                        throw SheaftException.Validation();

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderCancelledEvent(Id));

                    break;
                case PurchaseOrderStatus.Withdrawned:
                    if (Status == PurchaseOrderStatus.Withdrawned)
                        throw SheaftException.Validation();
                    if (Status == PurchaseOrderStatus.Refused)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotCancel_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotCancel_AlreadyIn_DeliveredStatus);
                    if (Status == PurchaseOrderStatus.Expired)
                        throw SheaftException.Validation();

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderWithdrawnedEvent(Id));

                    break;
                case PurchaseOrderStatus.Refused:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotRefuse_AlreadyIn_CancelledStatus);
                    if (Status == PurchaseOrderStatus.Refused)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotRefuse_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw SheaftException.Validation(MessageKind
                            .PurchaseOrder_CannotRefuse_AlreadyIn_DeliveredStatus);

                    DroppedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderRefusedEvent(Id));
                    break;

                case PurchaseOrderStatus.Expired:
                    if (Status != PurchaseOrderStatus.Waiting)
                        throw SheaftException.Validation();

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
                throw SheaftException.Validation(MessageKind.PurchaseOrder_CannotAddProduct_NotIn_WaitingStatus);

            if (product == null)
                throw SheaftException.Validation(MessageKind.PurchaseOrder_CannotAddProduct_Product_NotFound);

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

            LinesCount = Products.Count;
            ProductsCount = Products.Sum(p => p.Quantity);
            ReturnablesCount = Products.Sum(p => p.ReturnablesCount);

            TotalReturnableWholeSalePrice =
                Math.Round(Products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableWholeSalePrice.Value : 0),
                    DIGITS_COUNT);
            TotalReturnableVatPrice =
                Math.Round(Products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableVatPrice.Value : 0),
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