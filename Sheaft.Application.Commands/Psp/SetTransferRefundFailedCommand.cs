using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetTransferRefundFailedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-transfer-refund-failed";

        [JsonConstructor]
        public SetTransferRefundFailedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
