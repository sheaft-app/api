using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckCreatedPayoutTransactionCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-payout-transaction-created";

        [JsonConstructor]
        public CheckCreatedPayoutTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
