using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetUboDeclarationValidatedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-ubo-declaration-validated";

        [JsonConstructor]
        public SetUboDeclarationValidatedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
