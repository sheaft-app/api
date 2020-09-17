using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetDocumentValidationCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-document-validation-asked";

        [JsonConstructor]
        public SetDocumentValidationCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
