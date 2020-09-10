using Sheaft.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Order : IEntity
    {
        private List<PurchaseOrder> _purchaseOrders;
        private List<OrderTransaction> _transactions;

        protected Order()
        {
        }

        public Order(Guid id, User user, decimal donation = 0)
        {
            Id = id;

            Donation = donation;
            User = user;

            _purchaseOrders = new List<PurchaseOrder>();
            _transactions = new List<OrderTransaction>();
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal Donation { get; set; }
        public virtual User User { get; set; }

        public virtual IReadOnlyCollection<PurchaseOrder> PurchaseOrders
        {
            get => _purchaseOrders?.AsReadOnly();
        }

        public virtual IReadOnlyCollection<OrderTransaction> Transactions
        {
            get => _transactions?.AsReadOnly();
        }

        public void AddPurchaseOrder(PurchaseOrder entity)
        {
            if (PurchaseOrders == null)
                _purchaseOrders = new List<PurchaseOrder>();

            _purchaseOrders.Add(entity);
            RefreshPrices();
        }

        private void RefreshPrices()
        {
            TotalWholeSalePrice = PurchaseOrders.Sum(c => c.TotalWholeSalePrice);
            TotalVatPrice = PurchaseOrders.Sum(c => c.TotalVatPrice);
            TotalOnSalePrice = PurchaseOrders.Sum(c => c.TotalOnSalePrice);
        }
    }
}