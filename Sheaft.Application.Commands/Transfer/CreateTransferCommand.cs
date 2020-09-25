using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateTransferCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "command-create-transfer";

        [JsonConstructor]
        public CreateTransferCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
    }
}
