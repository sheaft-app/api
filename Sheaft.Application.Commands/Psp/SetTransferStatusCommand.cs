using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class SetTransferStatusCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-transfer-status";

        [JsonConstructor]
        public SetTransferStatusCommand(RequestUser requestUser, PspEventKind kind, string identifier, DateTimeOffset executedOn)
            : base(requestUser, kind, identifier, executedOn)
        {
        }
    }
}
