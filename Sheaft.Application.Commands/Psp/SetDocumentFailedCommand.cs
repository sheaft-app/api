using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetDocumentFailedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-document-failed";

        [JsonConstructor]
        public SetDocumentFailedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
