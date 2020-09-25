using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class RefreshTransferStatusCommand : Command<TransactionStatus>
    {
        public const string QUEUE_NAME = "command-refresh-transfer-status";

        [JsonConstructor]
        public RefreshTransferStatusCommand(RequestUser requestUser, string identifier) : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
