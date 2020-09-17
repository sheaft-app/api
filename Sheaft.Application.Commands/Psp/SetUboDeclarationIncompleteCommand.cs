using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetUboDeclarationIncompleteCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-ubo-declaration-incomplete";

        [JsonConstructor]
        public SetUboDeclarationIncompleteCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
