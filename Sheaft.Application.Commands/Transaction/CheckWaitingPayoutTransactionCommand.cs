using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckWaitingPayoutTransactionCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-payout-transaction-waiting";

        [JsonConstructor]
        public CheckWaitingPayoutTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
