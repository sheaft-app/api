using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class SetDeclarationStatusCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-declaration-status";

        [JsonConstructor]
        public SetDeclarationStatusCommand(RequestUser requestUser, PspEventKind kind, string identifier, DateTimeOffset executedOn)
            : base(requestUser, kind, identifier, executedOn)
        {
        }
    }
}
