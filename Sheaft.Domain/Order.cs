using Sheaft.Domain.Interop;
using Sheaft.Domain.Enums;
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
        private List<Payin> _payins;

        protected Order()
        {
        }

        public Order(Guid id, DonationKind kind, User user, IDictionary<Product, int> orderProducts, decimal fixedAmount, decimal percent)
        {
            Id = id;
            User = user;
            FeesFixedAmount = fixedAmount;
            FeesPercent = percent;
            DonationKind = kind;
            Status = OrderStatus.Created;

            _products = new List<OrderProduct>();
            _deliveries = new List<OrderDelivery>();
            _payins = new List<Payin>();

            SetProducts(orderProducts);
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
        public virtual IReadOnlyCollection<Payin> Payins => _payins?.AsReadOnly();

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

            RefreshFees();
        }

        private void RefreshFees()
        {
            Donation = 0;
            FeesPrice = GetFees(TotalOnSalePrice);

            switch (DonationKind)
            {
                case DonationKind.Rounded:
                    Donation = GetRoundedDonation();
                    break;
                case DonationKind.Euro:
                    Donation = 1;
                    break;
                case DonationKind.None:
                case DonationKind.Free:
                default:
                    Donation = 0;
                    break;
            }

            UpdateFees();
            TotalPrice = Math.Round(TotalOnSalePrice + Donation + FeesPrice - InternalFeesPrice, DIGITS_COUNT);
        }

        private decimal GetRoundedDonation()
        {
            var value = TotalOnSalePrice + FeesPrice;
            return Math.Ceiling(value) - value;
        }

        private void UpdateFees()
        {
            var total = TotalOnSalePrice + FeesPrice + Donation;
            var newFees = CalculateFees(total);

            InternalFeesPrice = Math.Round(newFees - FeesPrice, DIGITS_COUNT);
            FeesPrice = Math.Round(newFees, DIGITS_COUNT);
        }

        public decimal GetFees(decimal total)
        {
            var fees = CalculateFees(total);
            var mangofees = CalculateFees(total + fees);
            var increment = fees;

            while (total + mangofees > total + fees)
            {
                increment += 0.01m;
                fees = CalculateFees(total + increment);
                mangofees = CalculateFees(total + fees);
            }

            return Math.Round(fees, 2);
        }

        private decimal CalculateFees(decimal total)
        {
            return (FeesPercent * total) + FeesFixedAmount;
        }
    }
}