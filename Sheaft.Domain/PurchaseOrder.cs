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
        private List<PurchaseTransaction> _transactions;

        protected PurchaseOrder()
        {
        }

        public PurchaseOrder(Guid id, string reference, OrderStatusKind status, IDictionary<Product, int> lines, DeliveryMode delivery, DateTimeOffset expectedDeliveryDate, Producer vendor, User sender)
        {
            if (vendor == null)
                throw new ValidationException(MessageKind.PurchaseOrder_Vendor_Required);

            if (sender == null)
                throw new ValidationException(MessageKind.PurchaseOrder_Sender_Required);

            Id = id;

            _products = new List<PurchaseOrderProduct>();
            _transactions = new List<PurchaseTransaction>();

            SetSender(sender);
            SetVendor(vendor);
            SetExpectedDelivery(delivery, expectedDeliveryDate);

            SetReference(reference);
            SetOrderStatus(status);
            AddProducts(lines);
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
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public decimal TotalWeight { get; private set; }
        public OrderStatusKind Status { get; private set; }
        public virtual PurchaseOrderSender Sender { get; private set; }
        public virtual ExpectedDelivery ExpectedDelivery { get; private set; }
        public virtual PurchaseOrderVendor Vendor { get; private set; }
        public virtual IReadOnlyCollection<PurchaseOrderProduct> Products
        {
            get => _products?.AsReadOnly();
        }
        public virtual IReadOnlyCollection<PurchaseTransaction> Transactions
        {
            get => _transactions?.AsReadOnly();
        }

        public void SetReference(string newReference)
        {
            if (string.IsNullOrWhiteSpace(newReference))
                throw new ValidationException(MessageKind.PurchaseOrder_Reference_Required);

            Reference = newReference;
        }

        private void SetOrderStatus(OrderStatusKind newStatus)
        {
            switch (newStatus)
            {
                case OrderStatusKind.Accepted:
                    if (Status != OrderStatusKind.Waiting)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotAccept_NotIn_WaitingStatus);
                    break;
                case OrderStatusKind.Completed:
                    if (Status != OrderStatusKind.Processing)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotComplete_NotIn_ProcessingStatus);
                    break;
                case OrderStatusKind.Shipping:
                    if (Status != OrderStatusKind.Completed)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotShip_NotIn_CompletedStatus);
                    break;
                case OrderStatusKind.Delivered:
                    if (Status != OrderStatusKind.Completed && Status != OrderStatusKind.Shipping)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotDeliver_NotIn_CompletedOrShippingStatus);
                    break;
                case OrderStatusKind.Cancelled:
                    if (Status == OrderStatusKind.Cancelled)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_CancelledStatus);
                    if (Status == OrderStatusKind.Refused)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_RefusedStatus);
                    if (Status == OrderStatusKind.Delivered)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotCancel_AlreadyIn_DeliveredStatus);
                    break;
                case OrderStatusKind.Refused:
                    if (Status == OrderStatusKind.Cancelled)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_CancelledStatus);
                    if (Status == OrderStatusKind.Refused)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_RefusedStatus);
                    if (Status == OrderStatusKind.Delivered)
                        throw new ValidationException(MessageKind.PurchaseOrder_CannotRefuse_AlreadyIn_DeliveredStatus);
                    break;
            }

            Status = newStatus;
        }

        public void SetComment(string comment)
        {
            Comment = comment;
        }

        public void AddProducts(IDictionary<Product, int> products)
        {
            foreach (var product in products)
                AddProduct(product.Key, product.Value);
        }

        public void AddProduct(Product product, int quantity)
        {
            if (Status != OrderStatusKind.Waiting)
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
            if (Status != OrderStatusKind.Waiting)
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
            if (Status != OrderStatusKind.Waiting)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotRemoveProduct_NotIn_WaitingStatus);

            var productLine = _products.SingleOrDefault(p => p.Id == productId);
            if (productLine == null)
                throw new ValidationException(MessageKind.PurchaseOrder_CannotRemoveProduct_Product_NotFound);

            _products.Remove(productLine);
            RefreshOrder();
        }

        public void Ship()
        {
            SetOrderStatus(OrderStatusKind.Shipping);
        }

        public void Deliver()
        {
            SetOrderStatus(OrderStatusKind.Delivered);
            ExpectedDelivery.SetDeliveredDate(DateTimeOffset.UtcNow);
        }

        public void Cancel(string reason)
        {
            SetOrderStatus(OrderStatusKind.Cancelled);
            Reason = reason;
        }

        public void Refuse(string reason)
        {
            SetOrderStatus(OrderStatusKind.Refused);
            Reason = reason;
        }

        public void Process()
        {
            SetOrderStatus(OrderStatusKind.Processing);
        }

        public void Complete()
        {
            SetOrderStatus(OrderStatusKind.Completed);
        }

        public void Accept()
        {
            SetOrderStatus(OrderStatusKind.Accepted);
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

        protected void RefreshOrder()
        {
            LinesCount = _products.Count;
            ProductsCount = _products.Sum(p => p.Quantity);
            TotalWholeSalePrice = Math.Round(_products.Sum(p => p.TotalWholeSalePrice), DIGITS_COUNT);
            TotalVatPrice = Math.Round(_products.Sum(p => p.TotalVatPrice), DIGITS_COUNT);
            TotalOnSalePrice = Math.Round(_products.Sum(p => p.TotalOnSalePrice), DIGITS_COUNT);
            TotalWeight = Math.Round(_products.Where(p => p.TotalWeight.HasValue).Sum(p => p.TotalWeight) ?? 0, DIGITS_COUNT);
        }
    }
}