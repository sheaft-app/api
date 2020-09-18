using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class SetPayoutStatusCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payout-status";

        [JsonConstructor]
        public SetPayoutStatusCommand(RequestUser requestUser, PspEventKind kind, string identifier, DateTimeOffset executedOn)
            : base(requestUser, kind, identifier, executedOn)
        {
        }
    }
}
