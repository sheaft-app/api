using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreatePurchaseOrderTransferCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "command-create-purchaseorder-transfer";

        [JsonConstructor]
        public CreatePurchaseOrderTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
