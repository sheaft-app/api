using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetPayinFailedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payin-failed";

        [JsonConstructor]
        public SetPayinFailedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
