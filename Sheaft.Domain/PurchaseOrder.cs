using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using Sheaft.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class PurchaseOrder : IEntity
    {
        private const int DIGITS_COUNT = 2;
        private List<PurchaseOrderProduct> _products;

        protected PurchaseOrder()
        {
        }

        public PurchaseOrder(Guid id, string reference, PurchaseOrderStatusKind status, Producer producer, Order order)
        {
            if (producer == null)
                throw new ValidationException(MessageKind.PurchaseOrder_Vendor_Required);

            Id = id;

            _products = new List<PurchaseOrderProduct>();

            SetSender(order.User);
            SetVendor(producer);

            var delivery = order.Deliveries.FirstOrDefault(d => d.DeliveryMode.Producer.Id == producer.Id);
            SetExpectedDelivery(delivery.DeliveryMode, delivery.ExpectedDeliveryDate);
            SetComment(delivery.Comment);

            SetReference(reference);
            SetOrderStatus(status);

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
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public decimal TotalWeight { get; private set; }
        public decimal TotalReturnableOnSalePrice { get; set; }
        public decimal TotalReturnableWholeSalePrice { get; set; }
        public decimal TotalReturnableVatPrice { get; set; }
        public PurchaseOrderStatusKind Status { get; private set; }
        public virtual Order Order { get; private set; }
        public virtual PurchaseOrderSender Sender { get; private set; }
        public virtual ExpectedDelivery ExpectedDelivery { get; private set; }
        public virtual PurchaseOrderVendor Vendor { get; private set; }
        public virtual IReadOnlyCollection<PurchaseOrderProduct> Products => _products?.AsReadOnly();

        public void SetReference(string newReference)
        {
            if (string.IsNullOrWhiteSpace(newReference))
                throw new ValidationException(MessageKind.PurchaseOrder_Reference_Required);

            Reference = newReference;
        }

        private void SetOrderStatus(PurchaseOrderStatusKind newStatus)
        {
            switch (newStatus)
            {
                case PurchaseOrderStatusKind.Accepted:
                    if (Status != PurchaseOrderStatusKind.Waiting)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotAccept_NotIn_WaitingStatus);
                    break;
                case PurchaseOrderStatusKind.Completed:
                    if (Status != PurchaseOrderStatusKind.Processing)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotComplete_NotIn_ProcessingStatus);
                    break;
                case PurchaseOrderStatusKind.Shipping:
                    if (Status != PurchaseOrderStatusKind.Completed)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotShip_NotIn_CompletedStatus);
                    break;
                case PurchaseOrderStatusKind.Delivered:
                    if (Status != PurchaseOrderStatusKind.Completed && Status != PurchaseOrderStatusKind.Shipping)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotDeliver_NotIn_CompletedOrShippingStatus);
                    break;
                case PurchaseOrderStatusKind.Cancelled:
                    if (Status == PurchaseOrderStatusKind.Cancelled)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_CancelledStatus);
                    if (Status == PurchaseOrderStatusKind.Refused)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatusKind.Delivered)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_DeliveredStatus);
                    break;
                case PurchaseOrderStatusKind.Refused:
                    if (Status == PurchaseOrderStatusKind.Cancelled)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_CancelledStatus);
                    if (Status == PurchaseOrderStatusKind.Refused)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_RefusedStatus);
                    if (Status == PurchaseOrderStatusKind.Delivered)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_DeliveredStatus);
                    break;
            }

            Status = newStatus;
        }

        public void SetComment(string comment)
        {
            Comment = comment;
        }

        public void AddProduct(ProductRow product)
        {
            if (Status != PurchaseOrderStatusKind.Waiting)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotAddProduct_NotIn_WaitingStatus);

            if (product == null)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotAddProduct_Product_NotFound);

            var productLine = _products.SingleOrDefault(p => p.Id == product.Id);
            if (productLine == null)
                _products.Add(new PurchaseOrderProduct(product));
            else
                productLine.SetQuantity(product.Quantity);

            RefreshOrder();
        }

        public void AddProduct(Product product, int quantity)
        {
            if (Status != PurchaseOrderStatusKind.Waiting)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotAddProduct_NotIn_WaitingStatus);

            if (product == null)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotAddProduct_Product_NotFound);

            var productLine = _products.SingleOrDefault(p => p.Id == product.Id);
            if (productLine == null)
                _products.Add(new PurchaseOrderProduct(product, quantity));
            else
                productLine.SetQuantity(quantity);

            RefreshOrder();
        }

        public void ChangeProductQuantity(Guid productId, int quantity)
        {
            if (Status != PurchaseOrderStatusKind.Waiting)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotChangeProductQuantity_NotIn_WaitingStatus);

            var productLine = _products.SingleOrDefault(p => p.Id == productId);
            if (productLine == null)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotChangeProductQuantity_Product_NotFound);

            productLine.SetQuantity(quantity);
            RefreshOrder();
        }

        public void RemoveProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
                RemoveProduct(product.Id);
        }

        public void RemoveProduct(Guid productId)
        {
            if (Status != PurchaseOrderStatusKind.Waiting)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotRemoveProduct_NotIn_WaitingStatus);

            var productLine = _products.SingleOrDefault(p => p.Id == productId);
            if (productLine == null)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotRemoveProduct_Product_NotFound);

            _products.Remove(productLine);
            RefreshOrder();
        }

        public void Ship()
        {
            SetOrderStatus(PurchaseOrderStatusKind.Shipping);
        }

        public void Deliver()
        {
            SetOrderStatus(PurchaseOrderStatusKind.Delivered);
            ExpectedDelivery.SetDeliveredDate(DateTimeOffset.UtcNow);
        }

        public void Cancel(string reason)
        {
            SetOrderStatus(PurchaseOrderStatusKind.Cancelled);
            Reason = reason;
        }

        public void Refuse(string reason)
        {
            SetOrderStatus(PurchaseOrderStatusKind.Refused);
            Reason = reason;
        }

        public void Process()
        {
            SetOrderStatus(PurchaseOrderStatusKind.Processing);
        }

        public void Complete()
        {
            SetOrderStatus(PurchaseOrderStatusKind.Completed);
        }

        public void Accept()
        {
            SetOrderStatus(PurchaseOrderStatusKind.Accepted);
        }

        private void SetExpectedDelivery(DeliveryMode delivery, DateTimeOffset expectedDate)
        {
            ExpectedDelivery = new ExpectedDelivery(delivery, expectedDate);
        }

        public void SetSender(User sender)
        {
            Sender = new PurchaseOrderSender(sender);
        }

        public void SetVendor(Producer vendor)
        {
            Vendor = new PurchaseOrderVendor(vendor);
        }

        private void RefreshOrder()
        {
            TotalWholeSalePrice = Math.Round(_products.Sum(p => p.TotalWholeSalePrice), DIGITS_COUNT);
            TotalVatPrice = Math.Round(_products.Sum(p => p.TotalVatPrice), DIGITS_COUNT);
            TotalOnSalePrice = Math.Round(_products.Sum(p => p.TotalOnSalePrice), DIGITS_COUNT);

            TotalWeight = Math.Round(_products.Where(p => p.TotalWeight.HasValue).Sum(p => p.TotalWeight) ?? 0, DIGITS_COUNT);

            LinesCount = _products.Count();
            ProductsCount = _products.Sum(p => p.Quantity);
            ReturnablesCount = _products.Sum(p => p.ReturnablesCount);

            TotalReturnableWholeSalePrice = Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableWholeSalePrice.Value : 0), DIGITS_COUNT);
            TotalReturnableVatPrice = Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableVatPrice.Value : 0), DIGITS_COUNT);
            TotalReturnableOnSalePrice = Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableOnSalePrice.Value : 0), DIGITS_COUNT);
        }
    }
}