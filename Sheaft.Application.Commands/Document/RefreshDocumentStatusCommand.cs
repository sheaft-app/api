using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class RefreshDocumentStatusCommand : Command<bool>
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
