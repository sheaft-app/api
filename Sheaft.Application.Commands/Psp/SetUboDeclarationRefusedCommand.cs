using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetUboDeclarationRefusedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-ubo-declaration-refused";

        [JsonConstructor]
        public SetUboDeclarationRefusedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
