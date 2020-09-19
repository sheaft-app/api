using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckCreatedTransferTransactionCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-transfer-transaction-created";

        [JsonConstructor]
        public CheckCreatedTransferTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
