using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateTransferTransactionCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateTransferTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid PurchaseOrderId { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
    }
}
