using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetPayinRefundSucceededCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payin-refund-succeeded";

        [JsonConstructor]
        public SetPayinRefundSucceededCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
