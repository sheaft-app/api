using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderCreatedEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorders-created";
        public const string MAILING_TEMPLATE_ID_VENDOR = "d-2336303e2ec74491a3e76ab2b7c0a09f";
        public const string MAILING_TEMPLATE_ID_SENDER = "d-eba486af09404ea9b1601213be40cb67";

        public PurchaseOrderCreatedEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
