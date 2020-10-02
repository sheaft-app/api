using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class CreateTransferFailedEvent : Event
    {
        public const string MAILING_TEMPLATE_ID = "";

        [JsonConstructor]
        public CreateTransferFailedEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
