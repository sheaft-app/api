using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class RefreshTransferRefundStatusCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-refresh-transfer-refund-status";

        [JsonConstructor]
        public RefreshTransferRefundStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
