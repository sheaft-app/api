using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetTransferSucceededCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-transfer-succeeded";

        [JsonConstructor]
        public SetTransferSucceededCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
