using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetTransferRefundSucceededCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-transfer-refund-succeeded";

        [JsonConstructor]
        public SetTransferRefundSucceededCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
