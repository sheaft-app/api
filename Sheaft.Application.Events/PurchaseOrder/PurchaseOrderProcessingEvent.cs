using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class PurchaseOrderProcessingEvent : Event
    {
        public const string QUEUE_NAME = "eventpurchaseorderprocessing";

        public PurchaseOrderProcessingEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
