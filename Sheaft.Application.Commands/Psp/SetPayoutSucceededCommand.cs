using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetPayoutSucceededCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payout-succeeded";

        [JsonConstructor]
        public SetPayoutSucceededCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
