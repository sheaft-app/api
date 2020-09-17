using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetPayinRefundFailedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payin-refund-failed";

        [JsonConstructor]
        public SetPayinRefundFailedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
