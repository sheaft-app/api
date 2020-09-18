using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Order : IEntity
    {
        private const int DIGITS_COUNT = 2;
        private List<OrderProduct> _products;
        private List<OrderDelivery> _deliveries;

        protected Order()
        {
        }

        public Order(Guid id, User user, IDictionary<Product, int> orderProducts, IEnumerable<Tuple<DeliveryMode, DateTimeOffset, string>> orderDeliveries)
        {
            Id = id;
            Donation = 0;
            User = user;

            _products = new List<OrderProduct>();
            _deliveries = new List<OrderDelivery>();
            Status = OrderStatusKind.Created;

            SetProducts(orderProducts);
            SetDeliveries(orderDeliveries);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public OrderStatusKind Status { get; private set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public decimal TotalReturnableWholeSalePrice { get; private set; }
        public decimal TotalReturnableVatPrice { get; private set; }
        public decimal TotalReturnableOnSalePrice { get; private set; }
        public decimal TotalWeight { get; private set; }
        public int ReturnablesCount { get; private set; }
        public int LinesCount { get; private set; }
        public int ProductsCount { get; private set; }
        public decimal Donation { get; private set; }
        public decimal Fees { get; private set; }
        public virtual User User { get; private set; }
        public virtual IReadOnlyCollection<OrderProduct> Products => _products?.AsReadOnly();
        public virtual IReadOnlyCollection<OrderDelivery> Deliveries => _deliveries?.AsReadOnly();

        public void SetStatus(OrderStatusKind status)
        {
            Status = status;
        }

        public void SetProducts(IDictionary<Product, int> orderProducts)
        {
            if (Products != null)
                _products = new List<OrderProduct>();

            foreach (var orderProduct in orderProducts)
            {
                _products.Add(new OrderProduct(orderProduct.Key, orderProduct.Value));
            }

            RefreshOrder();
        }

        public void SetDeliveries(IEnumerable<Tuple<DeliveryMode, DateTimeOffset, string>> orderDeliveries)
        {
            if (Deliveries != null)
                _deliveries = new List<OrderDelivery>();

            foreach (var orderDelivery in orderDeliveries)
            {
                _deliveries.Add(new OrderDelivery(orderDelivery.Item1, orderDelivery.Item2, orderDelivery.Item3));
            }
        }

        public void RefreshFees(int fixedAmount, decimal percent)
        {
            Fees = Math.Round(fixedAmount + percent * TotalWholeSalePrice, DIGITS_COUNT);
        }

        public void SetDonation(decimal donation)
        {
            Donation = donation;
        }

        public void SetFees(decimal fees)
        {
            Fees = fees;
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