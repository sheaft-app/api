using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderCancelledByVendorEvent : Event
    {
        public const string QUEUE_NAME = "eventpurchaseordercancelledbyvendor";
        public const string MAILING_TEMPLATE_ID = "d-5a9e89b8b0564e8bba9ed6614e972454";

        public PurchaseOrderCancelledByVendorEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
