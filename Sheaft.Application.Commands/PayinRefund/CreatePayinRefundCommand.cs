using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class CreatePayinRefundCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "command-create-payin-refund";

        [JsonConstructor]
        public CreatePayinRefundCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
