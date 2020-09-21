using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class SetDocumentStatusCommand : PspCommand
    {
        public const string QUEUE_NAME = "command-psp-set-document-status";

        [JsonConstructor]
        public SetDocumentStatusCommand(RequestUser requestUser, PspEventKind kind, string identifier, DateTimeOffset executedOn)
            : base(requestUser, kind, identifier, executedOn)
        {

        }
    }
}
