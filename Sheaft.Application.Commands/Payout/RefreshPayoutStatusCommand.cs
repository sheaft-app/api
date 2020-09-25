using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class RefreshPayoutStatusCommand : Command<TransactionStatus>
    {
        public const string QUEUE_NAME = "command-refresh-payout-status";

        [JsonConstructor]
        public RefreshPayoutStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
