using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class RefreshPayoutRefundStatusCommand : Command<TransactionStatus>
    {
        public const string QUEUE_NAME = "command-refresh-payout-refund-status";

        [JsonConstructor]
        public RefreshPayoutRefundStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
