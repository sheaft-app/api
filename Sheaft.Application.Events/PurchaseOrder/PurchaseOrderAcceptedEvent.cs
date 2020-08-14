using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderAcceptedEvent : Event
    {
        public const string QUEUE_NAME = "event-purchaseorders-accepted";
        public const string MAILING_TEMPLATE_ID = "d-63757e1bde1c4942b28e50053a430167";

        public PurchaseOrderAcceptedEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
