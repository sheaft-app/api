using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckWaitingTransferTransactionCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-transfer-transaction-waiting";

        [JsonConstructor]
        public CheckWaitingTransferTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
