using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetPayoutFailedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payout-failed";

        [JsonConstructor]
        public SetPayoutFailedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
