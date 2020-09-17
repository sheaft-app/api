using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetDocumentSucceededCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-document-succeeded";

        [JsonConstructor]
        public SetDocumentSucceededCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
