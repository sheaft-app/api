﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Order;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Order : IEntity, IHasDomainEvent
    {
        private const int DIGITS_COUNT = 2;

        protected Order()
        {
        }

        public Order(Guid id, DonationKind kind, IEnumerable<Tuple<Product, Guid, int>> orderProducts,
            decimal fixedAmount = 0, decimal percent = 0, decimal vatPercent = 0, User user = null)
        {
            Id = id;
            User = user;
            UserId = user?.Id;
            FeesFixedAmount = fixedAmount;
            FeesPercent = percent;
            FeesVatPercent = vatPercent;
            DonationKind = kind;
            Status = OrderStatus.Created;

            Products = new List<OrderProduct>();
            Deliveries = new List<OrderDelivery>();
            PurchaseOrders = new List<PurchaseOrder>();

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
        public int DeliveriesCount { get; private set; }
        public int PurchaseOrdersCount { get; private set; }
        public decimal Donation { get; private set; }
        public decimal FeesPrice { get; private set; }
        public decimal DonationFeesPrice { get; private set; }
        public bool Processed { get; private set; }
        public decimal DeliveryFeesWholeSalePrice { get; private set; }
        public decimal DeliveryFeesVatPrice { get; private set; }
        public decimal DeliveryFeesOnSalePrice { get; private set; }
        public Guid? UserId { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<OrderProduct> Products { get; private set; }
        public virtual ICollection<OrderDelivery> Deliveries { get; private set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; private set; }

        public void AssignToUser(User user)
        {
            if (User != null)
                throw SheaftException.Conflict("Un utilisateur est déjà assigné à ce panier.");

            User = user;
        }

        public PurchaseOrder AddPurchaseOrder(int reference, Producer producer)
        {
            if (PurchaseOrders == null)
                PurchaseOrders = new List<PurchaseOrder>();

            var purchaseOrder =
                new PurchaseOrder(Guid.NewGuid(), reference, PurchaseOrderStatus.Waiting, producer, this);
            PurchaseOrders.Add(purchaseOrder);

            PurchaseOrdersCount = PurchaseOrders.Count;
            return purchaseOrder;
        }

        public void SetStatus(OrderStatus status)
        {
            if (Status == OrderStatus.Refused || Status == OrderStatus.Confirmed)
                return;

            switch (status)
            {
                case OrderStatus.Waiting:
                    ExpiredOn = null;
                    break;
                case OrderStatus.Expired:
                    ExpiredOn = DateTimeOffset.UtcNow;
                    break;
                case OrderStatus.Validated:
                    ExpiredOn = null;
                    DomainEvents.Add(new OrderValidatedEvent(Id));
                    break;
                case OrderStatus.Refused:
                    ExpiredOn = null;
                    break;
                case OrderStatus.Confirmed:
                    ExpiredOn = null;
                    DomainEvents.Add(new OrderConfirmedEvent(Id));
                    break;
            }

            Status = status;
        }

        public void SetProducts(IEnumerable<Tuple<Product, Guid, int>> orderProducts)
        {
            if (Products == null || Products.Any())
                Products = new List<OrderProduct>();

            foreach (var orderProduct in orderProducts)
            {
                var product = new OrderProduct(orderProduct.Item1, orderProduct.Item2, orderProduct.Item3);
                if (product.Quantity > 0)
                    Products.Add(product);
            }

            RefreshOrder();
        }

        public void SetDeliveries(IEnumerable<Tuple<DeliveryMode, DateTimeOffset, string>> orderDeliveries)
        {
            if (Deliveries == null || Deliveries.Any())
                Deliveries = new List<OrderDelivery>();

            foreach (var orderDelivery in orderDeliveries)
                Deliveries.Add(new OrderDelivery(orderDelivery.Item1, orderDelivery.Item2, orderDelivery.Item3));

            DeliveryFeesWholeSalePrice = 0;
            DeliveryFeesVatPrice = 0;
            DeliveryFeesOnSalePrice = 0;
            
            foreach (var delivery in Deliveries)
            {
                var deliveryMode = delivery.DeliveryMode;

                if (deliveryMode.AcceptPurchaseOrdersWithAmountGreaterThan.HasValue &&
                    deliveryMode.AcceptPurchaseOrdersWithAmountGreaterThan.Value >
                    (TotalProductWholeSalePrice + TotalReturnableWholeSalePrice))
                    throw SheaftException.Validation(
                        $"Le montant de la commande pour {deliveryMode.Producer.Name} est inférieur au palier de {deliveryMode.AcceptPurchaseOrdersWithAmountGreaterThan.Value}€ fixé par le producteur.");

                if (deliveryMode.ApplyDeliveryFeesWhen is DeliveryFeesApplication.Always)
                {
                    DeliveryFeesWholeSalePrice += deliveryMode.DeliveryFeesWholeSalePrice ?? 0;
                    DeliveryFeesVatPrice += deliveryMode.DeliveryFeesVatPrice ?? 0;
                    DeliveryFeesOnSalePrice += deliveryMode.DeliveryFeesOnSalePrice ?? 0;
                }

                if (deliveryMode.ApplyDeliveryFeesWhen is DeliveryFeesApplication.TotalLowerThanPurchaseOrderAmount &&
                    deliveryMode.DeliveryFeesMinPurchaseOrdersAmount.HasValue &&
                    deliveryMode.DeliveryFeesMinPurchaseOrdersAmount.Value >
                    (TotalProductWholeSalePrice + TotalReturnableWholeSalePrice))
                {
                    DeliveryFeesWholeSalePrice += deliveryMode.DeliveryFeesWholeSalePrice ?? 0;
                    DeliveryFeesVatPrice += deliveryMode.DeliveryFeesVatPrice ?? 0;
                    DeliveryFeesOnSalePrice += deliveryMode.DeliveryFeesOnSalePrice ?? 0;
                }
            }

            DeliveriesCount = Deliveries?.Count ?? 0;
            RefreshOrder();
        }

        public void SetDonation(DonationKind kind)
        {
            DonationKind = kind;
            RefreshFees();
        }

        private void RefreshOrder()
        {
            TotalProductWholeSalePrice = Products.Sum(p => p.TotalProductWholeSalePrice);
            TotalProductVatPrice = Products.Sum(p => p.TotalProductVatPrice);
            TotalProductOnSalePrice = TotalProductWholeSalePrice + TotalProductVatPrice;

            TotalWeight = Products.Where(p => p.TotalWeight.HasValue).Sum(p => p.TotalWeight) ?? 0;

            LinesCount = Products.Select(p => p.Id).Distinct().Count();
            ProductsCount = Products.Sum(p => p.Quantity);
            ReturnablesCount = Products.Where(p => p.HasReturnable).Sum(p => p.Quantity);

            TotalReturnableWholeSalePrice = Products.Sum(p => p.HasReturnable ? p.TotalReturnableWholeSalePrice.Value : 0);
            TotalReturnableVatPrice = Products.Sum(p => p.HasReturnable ? p.TotalReturnableVatPrice.Value : 0);
            TotalReturnableOnSalePrice = TotalReturnableWholeSalePrice + TotalReturnableVatPrice;

            TotalWholeSalePrice = TotalProductWholeSalePrice + TotalReturnableWholeSalePrice + DeliveryFeesWholeSalePrice;
            TotalVatPrice = TotalProductVatPrice + TotalReturnableVatPrice + DeliveryFeesVatPrice;
            TotalOnSalePrice = TotalWholeSalePrice + TotalVatPrice + DeliveryFeesOnSalePrice;

            RefreshFees();
        }

        private void RefreshFees()
        {
            var prices = GetOrderFees(this, TotalOnSalePrice);

            FeesPrice = prices.FeesPrice;
            TotalPrice = prices.TotalPrice;
            DonationFeesPrice = prices.DonationFees;
            Donation = prices.Donation;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }

        private class FeesDto
        {
            public decimal FeesPrice { get; set; }
            public decimal DonationFees { get; set; }
        }

        public static OrderPrices GetOrderFees(Order order, decimal totalOnSalePrice)
        {
            var donate = 0M;
            var feesPrice = GetFees(totalOnSalePrice, order.FeesPercent, order.FeesFixedAmount, order.FeesVatPercent);

            switch (order.DonationKind)
            {
                case DonationKind.Rounded:
                    donate = GetRoundedDonation(totalOnSalePrice, feesPrice);
                    break;
                case DonationKind.Euro:
                    donate = 1;
                    break;
                default:
                    donate = 0;
                    break;
            }

            var results = UpdateFees(totalOnSalePrice, feesPrice, donate, order.FeesPercent, order.FeesFixedAmount,
                order.FeesVatPercent);
            
            var totalPrice = totalOnSalePrice + donate + results.FeesPrice - results.DonationFees;

            return new OrderPrices
            {
                Donation = donate,
                DonationFees = results.DonationFees,
                FeesPrice = results.FeesPrice,
                TotalPrice = totalPrice
            };
        }

        private static decimal GetRoundedDonation(decimal totalOnSalePrice, decimal feesPrice)
        {
            var value = totalOnSalePrice + feesPrice;
            return Math.Ceiling(value) - value;
        }

        private static FeesDto UpdateFees(decimal totalOnSalePrice, decimal feesPrice, decimal donate,
            decimal feesPercent, decimal feesFixedAmount, decimal feesVatPercent)
        {
            var total = totalOnSalePrice + feesPrice + donate;
            var newFees = CalculateFees(total, feesPercent, feesFixedAmount, feesVatPercent);

            var donationFees = Math.Round(newFees - feesPrice, DIGITS_COUNT, MidpointRounding.AwayFromZero);
            var newFeesPrice = Math.Round(newFees, DIGITS_COUNT, MidpointRounding.AwayFromZero);

            return new FeesDto { FeesPrice = newFeesPrice, DonationFees = donationFees };
        }

        private static decimal GetFees(decimal total, decimal feesPercent, decimal feesFixedAmount,
            decimal feesVatPercent)
        {
            if (feesPercent <= 0)
                return 0;
            
            var fees = CalculateFees(total, feesPercent, feesFixedAmount, feesVatPercent);
            var pspFees = CalculateFees(total + fees, feesPercent, feesFixedAmount, feesVatPercent);
            var increment = fees;

            while (total + pspFees > total + fees)
            {
                increment += 0.01m;
                fees = CalculateFees(total + increment, feesPercent, feesFixedAmount, feesVatPercent);
                pspFees = CalculateFees(total + fees, feesPercent, feesFixedAmount, feesVatPercent);
            }

            return Math.Round(fees, 2, MidpointRounding.AwayFromZero);
        }

        private static decimal CalculateFees(decimal total, decimal feesPercent, decimal feesFixedAmount,
            decimal feesVatPercent)
        {
            var fees = (feesPercent * total) + feesFixedAmount;
            return fees + fees * feesVatPercent;
        }

        public void SetAsProcessed()
        {
            if (Processed)
                throw SheaftException.Conflict("Ce panier a déjà été traité.");

            Processed = true;
        }
    }
}