using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class SetDocumentOutDatedCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-document-outdated";

        [JsonConstructor]
        public SetDocumentOutDatedCommand(RequestUser requestUser, string identifier, DateTimeOffset executedOn)
            : base(requestUser, identifier, executedOn)
        {
        }
    }
}
