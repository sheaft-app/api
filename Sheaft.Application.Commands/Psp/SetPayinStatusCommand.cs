using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class SetPayinStatusCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-payin-status";

        [JsonConstructor]
        public SetPayinStatusCommand(RequestUser requestUser, PspEventKind kind, string identifier, DateTimeOffset executedOn)
            : base(requestUser, kind, identifier, executedOn)
        {
        }
    }
}
