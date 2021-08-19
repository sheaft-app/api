using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class ExpectedPurchaseOrderDelivery
    {
        protected ExpectedPurchaseOrderDelivery()
        {
        }
        
        public ExpectedPurchaseOrderDelivery(OrderDelivery delivery, ExpectedAddress address)
            : this(delivery.DeliveryMode, delivery.ExpectedDeliveryDate, delivery.From, delivery.To, address)
        {
        }
        
        public ExpectedPurchaseOrderDelivery(DeliveryMode deliveryMode, DateTimeOffset expectedDeliveryDate, TimeSpan from, TimeSpan to, ExpectedAddress address)
        {
            DeliveryModeId = deliveryMode.Id;
            Address = address;
            Day = expectedDeliveryDate.DayOfWeek;
            From = from;
            Kind = deliveryMode.Kind;
            Name = deliveryMode.Name;
            To = to;
            ExpectedDeliveryDate = expectedDeliveryDate;
        }
        
        public string Name { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
        public DayOfWeek Day { get; private set; }
        public ExpectedAddress Address { get; private set; }
        public Guid DeliveryModeId { get; private set; }
        public decimal DeliveryFeesWholeSalePrice { get; private set; }
        public decimal DeliveryFeesVatPrice { get; private set; }
        public decimal DeliveryFeesOnSalePrice { get; private set; }

        public void ApplyDeliveryModeFees(DeliveryMode deliveryMode)
        {
            DeliveryFeesWholeSalePrice = deliveryMode.DeliveryFeesWholeSalePrice ?? 0;
            DeliveryFeesVatPrice = deliveryMode.DeliveryFeesVatPrice ?? 0;
            DeliveryFeesOnSalePrice = deliveryMode.DeliveryFeesOnSalePrice ?? 0;
        }
    }
}