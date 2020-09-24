using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class SetRefundPayoutStatusCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payout-refund-status";

        [JsonConstructor]
        public SetRefundPayoutStatusCommand(RequestUser requestUser, PspEventKind kind, string identifier, DateTimeOffset executedOn)
            : base(requestUser, kind, identifier, executedOn)
        {
        }
    }
}
