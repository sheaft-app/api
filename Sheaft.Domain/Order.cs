using Sheaft.Interop;
using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Models
{
    public class Order : IEntity
    {
        private List<PurchaseOrder> _purchaseOrders;

        protected Order()
        {
        }

        public Order(Guid id)
        {
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal Donation { get; set; }
        public decimal Fees { get; set; }
        public virtual User User { get; set; }

        public virtual IReadOnlyCollection<PurchaseOrder> PurchaseOrders
        {
            get => _purchaseOrders?.AsReadOnly();
        }

        public void Remove()
        {
        }

        public void Restore()
        {
            RemovedOn = null;
        }
    }
}