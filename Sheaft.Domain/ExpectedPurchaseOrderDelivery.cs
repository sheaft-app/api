using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class ExpectedPurchaseOrderDelivery : ExpectedDelivery
    {
        protected ExpectedPurchaseOrderDelivery()
        {
        }

        public ExpectedPurchaseOrderDelivery(DeliveryMode mode, DateTimeOffset expectedDeliveryDate)
            : base(mode, expectedDeliveryDate)
        {
            Name = mode.Name;
            Kind = mode.Kind;
            Address = mode.Address != null ? new ExpectedAddress(mode.Address.Line1, mode.Address.Line2, mode.Address.Zipcode, mode.Address.City, mode.Address.Country, mode.Address.Longitude, mode.Address.Latitude) : null;
        }

        public string Name { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DateTimeOffset? DeliveryStartedOn { get; private set; }
        public DateTimeOffset? DeliveredOn { get; private set; }
        public virtual ExpectedAddress Address { get; private set; }

        public void SetDeliveredDate(DateTimeOffset date)
        {
            DeliveredOn = date;
        }
    }
}