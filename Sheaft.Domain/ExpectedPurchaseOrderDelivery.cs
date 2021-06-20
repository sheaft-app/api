using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class ExpectedPurchaseOrderDelivery
    {
        protected ExpectedPurchaseOrderDelivery()
        {
        }
        
        public ExpectedPurchaseOrderDelivery(OrderDelivery delivery, ExpectedAddress address)
        {
            DeliveryModeId = delivery.DeliveryModeId;
            Address = address;
            Day = delivery.Day;
            From = delivery.From;
            Kind = delivery.DeliveryMode.Kind;
            Name = delivery.DeliveryMode.Name;
            To = delivery.To;
            ExpectedDeliveryDate = delivery.ExpectedDeliveryDate;
        }
        
        public string Name { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DateTimeOffset ExpectedDeliveryDate { get; private set; }
        public TimeSpan From { get; private set; }
        public TimeSpan To { get; private set; }
        public DayOfWeek Day { get; private set; }
        public ExpectedAddress Address { get; private set; }
        public Guid DeliveryModeId { get; private set; }
    }
}