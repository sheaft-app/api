using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreatePurchaseOrderTransferCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-create-purchaseorder-transfer";

        [JsonConstructor]
        public CreatePurchaseOrderTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
