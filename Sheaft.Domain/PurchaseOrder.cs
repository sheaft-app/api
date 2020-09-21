using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class PurchaseOrder : IEntity
    {
        private const int DIGITS_COUNT = 2;
        private List<PurchaseOrderProduct> _products;
        private List<TransferTransaction> _transactions;

        protected PurchaseOrder()
        {
        }

        public PurchaseOrder(Guid id, string reference, PurchaseOrderStatus status, Producer producer, Order order)
        {
            if (producer == null)
                throw new ValidationException(MessageKind.PurchaseOrder_Vendor_Required);

            Id = id;

            _products = new List<PurchaseOrderProduct>();
            _transactions = new List<TransferTransaction>();

            SetSender(order.User);
            SetVendor(producer);

            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == producer.Id);
            SetExpectedDelivery(delivery.DeliveryMode, delivery.ExpectedDeliveryDate);
            SetComment(delivery.Comment);

            SetReference(reference);
            SetStatus(status);

            var orderProducts = order.Products.Where(p => p.Producer.Id == producer.Id);
            foreach(var orderProduct in orderProducts)
            {
                AddProduct(orderProduct);
            }
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
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
        public virtual Order Order { get; private set; }
        public virtual PurchaseOrderSender Sender { get; private set; }
        public virtual ExpectedDelivery ExpectedDelivery { get; private set; }
        public virtual PurchaseOrderVendor Vendor { get; private set; }
        public virtual IReadOnlyCollection<PurchaseOrderProduct> Products => _products?.AsReadOnly();
        public virtual IReadOnlyCollection<TransferTransaction> Transactions => _transactions?.AsReadOnly();

        public void SetReference(string newReference)
        {
            if (string.IsNullOrWhiteSpace(newReference))
                throw new ValidationException(MessageKind.PurchaseOrder_Reference_Required);

            Reference = newReference;
        }

        private void SetStatus(PurchaseOrderStatus newStatus)
        {
            switch (newStatus)
            {
                case PurchaseOrderStatus.Accepted:
                    if (Status != PurchaseOrderStatus.Waiting)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotAccept_NotIn_WaitingStatus);
                    break;
                case PurchaseOrderStatus.Completed:
                    if (Status != PurchaseOrderStatus.Processing)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotComplete_NotIn_ProcessingStatus);
                    break;
                case PurchaseOrderStatus.Shipping:
                    if (Status != PurchaseOrderStatus.Completed)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotShip_NotIn_CompletedStatus);
                    break;
                case PurchaseOrderStatus.Delivered:
                    if (Status != PurchaseOrderStatus.Completed && Status != PurchaseOrderStatus.Shipping)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotDeliver_NotIn_CompletedOrShippingStatus);
                    break;
                case PurchaseOrderStatus.Cancelled:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_CancelledStatus);
                    if (Status == PurchaseOrderStatus.Refused)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_DeliveredStatus);
                    break;
                case PurchaseOrderStatus.Refused:
                    if (Status == PurchaseOrderStatus.Cancelled)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_CancelledStatus);
                    if (Status == PurchaseOrderStatus.Refused)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatus.Delivered)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_DeliveredStatus);
                    break;
            }

            Status = newStatus;
        }

        public void SetComment(string comment)
        {
            Comment = comment;
        }

        public void Ship()
        {
            SetStatus(PurchaseOrderStatus.Shipping);
        }

        public void Deliver()
        {
            SetStatus(PurchaseOrderStatus.Delivered);
            ExpectedDelivery.SetDeliveredDate(DateTimeOffset.UtcNow);
        }

        public void Cancel(string reason)
        {
            SetStatus(PurchaseOrderStatus.Cancelled);
            Reason = reason;
        }

        public void Refuse(string reason)
        {
            SetStatus(PurchaseOrderStatus.Refused);
            Reason = reason;
        }

        public void Process()
        {
            SetStatus(PurchaseOrderStatus.Processing);
        }

        public void Complete()
        {
            SetStatus(PurchaseOrderStatus.Completed);
        }

        public void Accept()
        {
            SetStatus(PurchaseOrderStatus.Accepted);
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
            ExpectedDelivery = new ExpectedDelivery(delivery, expectedDate);
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

            TotalWeight = Math.Round(_products.Where(p => p.TotalWeight.HasValue).Sum(p => p.TotalWeight) ?? 0, DIGITS_COUNT);

            LinesCount = _products.Count;
            ProductsCount = _products.Sum(p => p.Quantity);
            ReturnablesCount = _products.Sum(p => p.ReturnablesCount);

            TotalReturnableWholeSalePrice = Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableWholeSalePrice.Value : 0), DIGITS_COUNT);
            TotalReturnableVatPrice = Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableVatPrice.Value : 0), DIGITS_COUNT);
            TotalReturnableOnSalePrice = Math.Round(TotalReturnableWholeSalePrice + TotalReturnableVatPrice, DIGITS_COUNT);

            TotalWholeSalePrice = TotalProductWholeSalePrice + TotalReturnableWholeSalePrice;
            TotalVatPrice = TotalProductVatPrice + TotalReturnableVatPrice;
            TotalOnSalePrice = Math.Round(TotalWholeSalePrice + TotalVatPrice, DIGITS_COUNT);
        }
    }
}