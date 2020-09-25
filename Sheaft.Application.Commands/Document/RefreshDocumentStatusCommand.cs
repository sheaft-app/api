using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class RefreshDocumentStatusCommand : Command<DocumentStatus>
    {
        public const string QUEUE_NAME = "command-refresh-document-status";

        [JsonConstructor]
        public RefreshDocumentStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
