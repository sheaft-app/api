using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetUboDeclarationValidationCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-ubo-declaration-validation-asked";

        [JsonConstructor]
        public SetUboDeclarationValidationCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
