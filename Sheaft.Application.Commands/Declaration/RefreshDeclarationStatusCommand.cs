using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class RefreshDeclarationStatusCommand : Command<DeclarationStatus>
    {
        public const string QUEUE_NAME = "command-refresh-declaration-status";

        [JsonConstructor]
        public RefreshDeclarationStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
