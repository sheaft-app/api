using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetPayinSucceededCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payin-succeeded";

        [JsonConstructor]
        public SetPayinSucceededCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
