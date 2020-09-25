using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateTransferRefundCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "command-create-transfer-refund";

        [JsonConstructor]
        public CreateTransferRefundCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
