using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.PurchaseOrder;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PurchaseOrder : IEntity, IHasDomainEvent
    {
        private const int DIGITS_COUNT = 2;
        private List<PurchaseOrderProduct> _products;

        protected PurchaseOrder()
        {
        }

        public PurchaseOrder(Guid id, string reference, PurchaseOrderStatus status, Producer producer, Order order)
        {
            if (producer == null)
                throw new ValidationException(MessageKind.PurchaseOrder_Vendor_Required);

            Id = id;

            _products = new List<PurchaseOrderProduct>();
            DomainEvents = new List<DomainEvent> {new PurchaseOrderCreatedEvent(Id)};

            SetSender(order.User);
            SetVendor(producer);

            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == producer.Id);
            SetExpectedDelivery(delivery.DeliveryMode, delivery.ExpectedDelivery.ExpectedDeliveryDate);
            SetComment(delivery.Comment);

            SetReference(reference);
            SetStatus(status, false);

            var orderProducts = order.Products.Where(p => p.Producer.Id == producer.Id);
            foreach (var orderProduct in orderProducts)
            {
                AddProduct(orderProduct);
            }
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? AcceptedOn { get; private set; }
        public DateTimeOffset? CompletedOn { get; private set; }
        public DateTimeOffset? DeliveredOn { get; private set; }
        public DateTimeOffset? WithdrawnOn { get; private set; }
        public string Reference { get; private set; }
        public string Reason { get; private set; }
        public string Comment { get; private set; }
        public int LinesCount { get; private set; }
        public int ProductsCount { get; private set; }
        public int ReturnablesCount { get; private set; }
        public decimal TotalProductWholeSalePrice { get; private set; }
        public decimal TotalProductVatPrice { get; private set; }
        public decimal TotalProductOnSalePrice { get; private set; }
        public decimal TotalReturnableOnSalePrice { get; set; }
        public decimal TotalReturnableWholeSalePrice { get; set; }
        public decimal TotalReturnableVatPrice { get; set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public decimal TotalWeight { get; private set; }
        public PurchaseOrderStatus Status { get; private set; }
        public virtual PurchaseOrderSender Sender { get; private set; }
        public virtual ExpectedPurchaseOrderDelivery ExpectedDelivery { get; private set; }
        public virtual PurchaseOrderVendor Vendor { get; private set; }
        public virtual Transfer Transfer { get; private set; }
        public virtual IReadOnlyCollection<PurchaseOrderProduct> Products => _products?.AsReadOnly();

        public void SetReference(string newReference)
        {
            if (string.IsNullOrWhiteSpace(newReference))
                throw new ValidationException(MessageKind.PurchaseOrder_Reference_Required);

            Reference = newReference;
        }

        public void SetTransfer(Transfer transfer)
        {
            if (Transfer != null && Transfer.Status == TransactionStatus.Succeeded)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotSet_Transfer_AlreadySucceeded);

            Transfer = transfer;
        }

        private void SetStatus(PurchaseOrderStatus newStatus, bool skipNotification)
        {
            switch (newStatus)
            {
                case PurchaseOrderStatus.Accepted:
                    if (Status != PurchaseOrderStatus.Waiting)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotAccept_NotIn_WaitingStatus);

                    AcceptedOn = DateTimeOffset.UtcNow;
                    Status = PurchaseOrderStatus.Processing;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderAcceptedEvent(Id));

                    return;
                case PurchaseOrderStatus.Completed:
                    if (Status != PurchaseOrderStatus.Processing)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotComplete_NotIn_ProcessingStatus);

                    CompletedOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderCompletedEvent(Id));

                    break;
                case PurchaseOrderStatus.Shipping:
                    if (Status != PurchaseOrderStatus.Completed)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotShip_NotIn_CompletedStatus);

                    break;
                case PurchaseOrderStatus.Delivered:
                    if (Status != PurchaseOrderStatus.Completed && Status != PurchaseOrderStatus.Shipping)
                        throw new ValidationException(MessageKind
                            .PurchaseOrder_CannotDeliver_NotIn_CompletedOrShippingStatus);

                    DeliveredOn = DateTimeOffset.UtcNow;
                    break;
                case PurchaseOrderStatus.Cancelled:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_CancelledStatus);
                    if (Status == PurchaseOrderStatus.Refused)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_DeliveredStatus);
                    WithdrawnOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderCancelledEvent(Id));

                    break;
                case PurchaseOrderStatus.Refused:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_CancelledStatus);
                    if (Status == PurchaseOrderStatus.Refused)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_DeliveredStatus);
                    WithdrawnOn = DateTimeOffset.UtcNow;

                    if (!skipNotification)
                        DomainEvents.Add(new PurchaseOrderRefusedEvent(Id));

                    break;
            }

            Status = newStatus;
        }

        public void SetComment(string comment)
        {
            Comment = comment;
        }

        public void Ship(bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Shipping, skipNotification);
        }

        public void Deliver(bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Delivered, skipNotification);
            ExpectedDelivery.SetDeliveredDate(DateTimeOffset.UtcNow);
        }

        public void Cancel(string reason, bool skipNotification)
        {
            SetStatus(PurchaseOrderStatus.Cancelled, skipNotification);
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
            Sender = new PurchaseOrderSender(sender);
        }

        public void SetVendor(Producer vendor)
        {
            Vendor = new PurchaseOrderVendor(vendor);
        }

        private void SetExpectedDelivery(DeliveryMode delivery, DateTimeOffset expectedDate)
        {
            ExpectedDelivery = new ExpectedPurchaseOrderDelivery(delivery, expectedDate);
        }

        private void AddProduct(ProductRow product)
        {
            if (Status != PurchaseOrderStatus.Waiting)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotAddProduct_NotIn_WaitingStatus);

            if (product == null)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotAddProduct_Product_NotFound);

            var productLine = _products.SingleOrDefault(p => p.Id == product.Id);
            if (productLine != null)
                _products.Remove(productLine);

            _products.Add(new PurchaseOrderProduct(product));
            RefreshOrder();
        }

        private void RefreshOrder()
        {
            TotalProductWholeSalePrice = Math.Round(_products.Sum(p => p.TotalWholeSalePrice), DIGITS_COUNT);
            TotalProductVatPrice = Math.Round(_products.Sum(p => p.TotalVatPrice), DIGITS_COUNT);
            TotalProductOnSalePrice = Math.Round(TotalProductWholeSalePrice + TotalProductVatPrice, DIGITS_COUNT);

            TotalWeight = Math.Round(_products.Where(p => p.TotalWeight.HasValue).Sum(p => p.TotalWeight) ?? 0,
                DIGITS_COUNT);

            LinesCount = _products.Count;
            ProductsCount = _products.Sum(p => p.Quantity);
            ReturnablesCount = _products.Sum(p => p.ReturnablesCount);

            TotalReturnableWholeSalePrice =
                Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableWholeSalePrice.Value : 0),
                    DIGITS_COUNT);
            TotalReturnableVatPrice =
                Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableVatPrice.Value : 0),
                    DIGITS_COUNT);
            TotalReturnableOnSalePrice =
                Math.Round(TotalReturnableWholeSalePrice + TotalReturnableVatPrice, DIGITS_COUNT);

            TotalWholeSalePrice = TotalProductWholeSalePrice + TotalReturnableWholeSalePrice;
            TotalVatPrice = TotalProductVatPrice + TotalReturnableVatPrice;
            TotalOnSalePrice = Math.Round(TotalWholeSalePrice + TotalVatPrice, DIGITS_COUNT);
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}