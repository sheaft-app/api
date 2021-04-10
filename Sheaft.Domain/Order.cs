﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
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

        public Order(Guid id, DonationKind kind, IEnumerable<Tuple<Product, Guid, int>> orderProducts, decimal fixedAmount, decimal percent, decimal vatPercent, User user = null)
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
        public decimal Donation { get; private set; }
        public decimal FeesPrice { get; private set; }
        public decimal DonationFeesPrice { get; private set; }
        public virtual User User { get; private set; }
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

        public void SetStatus(OrderStatus status)
        {
            if (Status == OrderStatus.Refused || Status == OrderStatus.Validated)
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
                case OrderStatus.Refused:
                    ExpiredOn = null;
                    break;
            }

            Status = status;
        }

        public void SetProducts(IEnumerable<Tuple<Product, Guid, int>> orderProducts)
        {
            if (Products == null || Products.Any())
                _products = new List<OrderProduct>();

            foreach (var orderProduct in orderProducts)
            {
                var product = new OrderProduct(orderProduct.Item1, orderProduct.Item2, orderProduct.Item3);
                if(product.Quantity > 0)
                    _products.Add(product);
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
            var prices = GetOrderFees(this, TotalOnSalePrice);
            
            FeesPrice = prices.FeesPrice;
            TotalPrice = prices.TotalPrice;
            DonationFeesPrice = prices.DonationFees;
            Donation = prices.Donation;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
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

            var results = UpdateFees(totalOnSalePrice, feesPrice, donate, order.FeesPercent, order.FeesFixedAmount, order.FeesVatPercent);
            var totalPrice = Math.Round(totalOnSalePrice + donate + results.FeesPrice - results.DonationFees, DIGITS_COUNT);

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

        private static FeesDto UpdateFees(decimal totalOnSalePrice, decimal feesPrice, decimal donate, decimal feesPercent, decimal feesFixedAmount, decimal feesVatPercent)
        {
            var total = totalOnSalePrice + feesPrice + donate;
            var newFees = CalculateFees(total, feesPercent, feesFixedAmount, feesVatPercent);

            var donationFees = Math.Round(newFees - feesPrice, DIGITS_COUNT);
            var newFeesPrice = Math.Round(newFees, DIGITS_COUNT);

            return new FeesDto{ FeesPrice = newFeesPrice, DonationFees = donationFees};
        }

        private static decimal GetFees(decimal total, decimal feesPercent, decimal feesFixedAmount, decimal feesVatPercent)
        {
            var fees = CalculateFees(total, feesPercent, feesFixedAmount, feesVatPercent);
            var pspFees = CalculateFees(total + fees, feesPercent, feesFixedAmount, feesVatPercent);
            var increment = fees;

            while (total + pspFees > total + fees)
            {
                increment += 0.01m;
                fees = CalculateFees(total + increment, feesPercent, feesFixedAmount, feesVatPercent);
                pspFees = CalculateFees(total + fees, feesPercent, feesFixedAmount, feesVatPercent);
            }

            return Math.Round(fees, 2);
        }

        private static decimal CalculateFees(decimal total, decimal feesPercent, decimal feesFixedAmount, decimal feesVatPercent)
        {
            var fees = (feesPercent * total) + feesFixedAmount;
            return fees + fees * feesVatPercent;
        }
    }
}
