using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class PurchaseOrderDelivery
    {
        protected PurchaseOrderDelivery()
        {
        }
        
        public PurchaseOrderDelivery(Guid distributionId, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, PurchaseOrderDeliveryAddress address)
        {
            DistributionId = distributionId;
            Address = address;
            From = from;
            To = to;
            ExpectedDeliveryDate = expectedDeliveryDate;
        }

        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
        public PurchaseOrderDeliveryAddress Address { get; private set; }
        public Guid DistributionId { get; private set; }
        public decimal DeliveryFeesWholeSalePrice { get; private set; }
    }
}