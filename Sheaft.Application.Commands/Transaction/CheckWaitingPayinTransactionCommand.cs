using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckWaitingPayinTransactionCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-payin-transaction-waiting";

        [JsonConstructor]
        public CheckWaitingPayinTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
