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
        private List<PayinTransaction> _transactions;

        protected Order()
        {
        }

        public Order(Guid id, DonationKind kind, User user, IDictionary<Product, int> orderProducts, decimal fixedAmount, decimal percent)
        {
            Id = id;
            Donation = 0;
            User = user;
            FeesFixedAmount = fixedAmount;
            FeesPercent = percent;

            _products = new List<OrderProduct>();
            _deliveries = new List<OrderDelivery>();
            _transactions = new List<PayinTransaction>();
            Status = OrderStatus.Created;

            SetProducts(orderProducts);
            SetDonation(kind);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public OrderStatus Status { get; private set; }
        public DonationKind DonationKind { get; private set; }
        public decimal TotalProductWholeSalePrice { get; private set; }
        public decimal TotalProductVatPrice { get; private set; }
        public decimal TotalProductOnSalePrice { get; private set; }
        public decimal TotalReturnableWholeSalePrice { get; private set; }
        public decimal TotalReturnableVatPrice { get; private set; }
        public decimal TotalReturnableOnSalePrice { get; private set; }
        public decimal TotalWholeSalePrice { get; private set; }
        public decimal TotalVatPrice { get; private set; }
        public decimal TotalOnSalePrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        public decimal TotalWeight { get; private set; }
        public decimal FeesFixedAmount { get; private set; }
        public decimal FeesPercent { get; private set; }
        public int ReturnablesCount { get; private set; }
        public int LinesCount { get; private set; }
        public int ProductsCount { get; private set; }
        public decimal Donation { get; private set; }
        public decimal FeesPrice { get; private set; }
        public decimal InternalFeesPrice { get; private set; }
        public virtual User User { get; private set; }
        public virtual IReadOnlyCollection<OrderProduct> Products => _products?.AsReadOnly();
        public virtual IReadOnlyCollection<OrderDelivery> Deliveries => _deliveries?.AsReadOnly();
        public virtual IReadOnlyCollection<PayinTransaction> Transactions => _transactions?.AsReadOnly();

        public void SetStatus(OrderStatus status)
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

        public void SetDonation(DonationKind kind)
        {
            DonationKind = kind;
            RefreshFees();
        }

        private void RefreshOrder()
        {
            TotalProductWholeSalePrice = Math.Round(_products.Sum(p => p.TotalWholeSalePrice), DIGITS_COUNT);
            TotalProductVatPrice = Math.Round(_products.Sum(p => p.TotalVatPrice), DIGITS_COUNT);
            TotalProductOnSalePrice = Math.Round(_products.Sum(p => p.TotalOnSalePrice), DIGITS_COUNT);

            TotalWeight = Math.Round(_products.Where(p => p.TotalWeight.HasValue).Sum(p => p.TotalWeight) ?? 0, DIGITS_COUNT);

            LinesCount = _products.Count();
            ProductsCount = _products.Sum(p => p.Quantity);
            ReturnablesCount = _products.Sum(p => p.ReturnablesCount);

            TotalReturnableWholeSalePrice = Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableWholeSalePrice.Value : 0), DIGITS_COUNT);
            TotalReturnableVatPrice = Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableVatPrice.Value : 0), DIGITS_COUNT);
            TotalReturnableOnSalePrice = Math.Round(_products.Sum(p => p.ReturnablesCount > 0 ? p.TotalReturnableOnSalePrice.Value : 0), DIGITS_COUNT);

            TotalWholeSalePrice = TotalProductWholeSalePrice + TotalReturnableWholeSalePrice;
            TotalVatPrice = TotalProductVatPrice + TotalReturnableVatPrice;
            TotalOnSalePrice = TotalProductOnSalePrice + TotalReturnableOnSalePrice;

            RefreshFees();
        }

        private void RefreshFees()
        {
            switch (DonationKind)
            {
                case DonationKind.Rounded:
                    var value = TotalOnSalePrice + GetFees(TotalOnSalePrice);
                    Donation = Math.Ceiling(value) - value;
                    break;
                case DonationKind.Euro:
                    Donation = 1;
                    break;
                default:
                    Donation = 0;
                    break;
            }

            FeesPrice = GetFees(TotalOnSalePrice + Donation);
            TotalPrice = TotalOnSalePrice + Donation + FeesPrice + InternalFeesPrice;
        }

        public decimal GetFees(decimal total)
        {
            var fees = CalculateFees(total) + FeesFixedAmount;
            var mangofees = CalculateFees(total + fees) + FeesFixedAmount;
            var increment = fees;

            while (total + mangofees > total + fees)
            {
                increment += 0.01m;
                fees = CalculateFees(total + increment) + FeesFixedAmount;
                mangofees = CalculateFees(total + fees) + FeesFixedAmount;
            }

            return Math.Round(fees, 2);
        }

        private decimal CalculateFees(decimal total)
        {
            return FeesPercent * total;
        }
    }
}