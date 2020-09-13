using Sheaft.Interop;
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

            SetProducts(orderProducts);
            SetDeliveries(orderDeliveries);
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public decimal Donation { get; private set; }
        public decimal Fees { get; private set; }
        public virtual User User { get; private set; }
        public virtual IReadOnlyCollection<OrderProduct> Products => _products?.AsReadOnly();
        public virtual IReadOnlyCollection<OrderDelivery> Deliveries => _deliveries?.AsReadOnly();

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
        }
    }
}