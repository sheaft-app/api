using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetTransferFailedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-transfer-failed";

        [JsonConstructor]
        public SetTransferFailedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
