using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckCreatedPayinTransactionCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-payin-transaction-created";

        [JsonConstructor]
        public CheckCreatedPayinTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TransactionId { get; set; }
    }
}
