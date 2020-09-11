using Sheaft.Interop;
using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Models
{
    public class Order : IEntity
    {
        private List<OrderProduct> _products;
        private List<OrderDelivery> _deliveries;

        protected Order()
        {
        }

        public Order(Guid id, User user, IDictionary<Product, int> products, IEnumerable<Tuple<DeliveryMode, DateTimeOffset, string>> expectedDeliveries)
        {
            Id = id;

            Donation = 0;
            User = user;

            foreach(var product in products)
            {
                AddProduct(product.Key, product.Value);
            }

            foreach (var expectedDelivery in expectedDeliveries)
            {
                AddDelivery(expectedDelivery.Item1, expectedDelivery.Item2, expectedDelivery.Item3);
            }
        }

        private void AddDelivery(DeliveryMode deliveryMode, DateTimeOffset expectedDeliveryDate, string comment)
        {
            if (Deliveries == null)
                _deliveries = new List<OrderDelivery>();

            _deliveries.Add(new OrderDelivery(deliveryMode, expectedDeliveryDate, comment));
        }

        private void AddProduct(Product product, int quantity)
        {
            if (Products == null)
                _products = new List<OrderProduct>();

            _products.Add(new OrderProduct(product, quantity));
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
        public virtual IReadOnlyCollection<OrderProduct> Products
        {
            get => _products?.AsReadOnly();
        }
        public virtual IReadOnlyCollection<OrderDelivery> Deliveries
        {
            get => _deliveries?.AsReadOnly();
        }

        public void SetDonation(decimal donation)
        {
            Donation = donation;
        }

        public void SetFees(decimal fees)
        {
            Fees = fees;
        }
    }
}