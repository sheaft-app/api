using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Order : IEntity, IHasDomainEvent
    {
        private const int DIGITS_COUNT = 2;
        private List<OrderProduct> _products;
        private List<OrderDelivery> _deliveries;
        private List<PurchaseOrder> _purchaseOrders;

        protected Order()
        {
        }

        public Order(Guid id, DonationKind kind, IDictionary<Product, int> orderProducts, decimal fixedAmount, decimal percent, decimal vatPercent, User user = null)
        {
            Id = id;
            User = user;
            FeesFixedAmount = fixedAmount;
            FeesPercent = percent;
            FeesVatPercent = vatPercent;
            DonationKind = kind;
            Status = OrderStatus.Created;

            _products = new List<OrderProduct>();
            _deliveries = new List<OrderDelivery>();
            DomainEvents = new List<DomainEvent>();

            SetProducts(orderProducts);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? ExpiredOn { get; private set; }
        public OrderStatus Status { get; private set; }
        public DonationKind DonationKind { get; private set; }
        public string Reference { get; private set; }
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
        public decimal FeesVatPercent { get; private set; }
        public decimal FeesPercent { get; private set; }
        public int ReturnablesCount { get; private set; }
        public int LinesCount { get; private set; }
        public int ProductsCount { get; private set; }
        public decimal Donate { get; private set; }
        public decimal FeesPrice { get; private set; }
        public decimal InternalFeesPrice { get; private set; }
        public virtual User User { get; private set; }
        public virtual Payin Payin { get; private set; }
        public virtual Donation Donation { get; private set; }
        public virtual IReadOnlyCollection<OrderProduct> Products => _products?.AsReadOnly();
        public virtual IReadOnlyCollection<OrderDelivery> Deliveries => _deliveries?.AsReadOnly();
        public virtual IReadOnlyCollection<PurchaseOrder> PurchaseOrders => _purchaseOrders?.AsReadOnly();

        public void AssignToUser(User user)
        {
            if (User != null)
                throw SheaftException.Conflict();
            
            User = user;
        }

        public PurchaseOrder AddPurchaseOrder(string reference, Producer producer)
        {
            if (PurchaseOrders == null)
                _purchaseOrders = new List<PurchaseOrder>();

            var purchaseOrder = new PurchaseOrder(Guid.NewGuid(), reference, PurchaseOrderStatus.Waiting, producer, this);
            _purchaseOrders.Add(purchaseOrder);

            return purchaseOrder;
        }

        public void SetPayin(Payin payin)
        {
            if(Payin != null && Payin.Status == TransactionStatus.Succeeded)
                throw new ValidationException(MessageKind.Payin_CannotSet_Already_Succeeded);

            Payin = payin;
        }

        public void SetDonation(Donation donation)
        {
            if (Donation != null && Donation.Status == TransactionStatus.Succeeded)
                throw new ValidationException(MessageKind.Donation_CannotSet_Already_Succeeded);

            Donation = donation;
        }

        public void SetStatus(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Waiting:
                    ExpiredOn = null;
                    break;
                case OrderStatus.Expired:
                    ExpiredOn = DateTimeOffset.UtcNow;
                    break;
                case OrderStatus.Validated:
                case OrderStatus.Refused:
                    ExpiredOn = null;
                    break;
            }

            Status = status;
        }

        public void SetProducts(IDictionary<Product, int> orderProducts)
        {
            if (Products == null || Products.Any())
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

        public void SetReference(string reference)
        {
            if (reference == null)
                return;

            Reference = reference;
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
            Donate = 0;
            FeesPrice = GetFees(TotalOnSalePrice);

            switch (DonationKind)
            {
                case DonationKind.Rounded:
                    Donate = GetRoundedDonation();
                    break;
                case DonationKind.Euro:
                    Donate = 1;
                    break;
                case DonationKind.None:
                case DonationKind.Free:
                default:
                    Donate = 0;
                    break;
            }

            UpdateFees();
            TotalPrice = Math.Round(TotalOnSalePrice + Donate + FeesPrice - InternalFeesPrice, DIGITS_COUNT);
        }

        private decimal GetRoundedDonation()
        {
            var value = TotalOnSalePrice + FeesPrice;
            return Math.Ceiling(value) - value;
        }

        private void UpdateFees()
        {
            var total = TotalOnSalePrice + FeesPrice + Donate;
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
            var fees = (FeesPercent * total) + FeesFixedAmount;
            return fees + fees * FeesVatPercent;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}
