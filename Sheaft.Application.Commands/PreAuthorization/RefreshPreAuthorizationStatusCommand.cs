using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class RefreshPreAuthorizationStatusCommand : Command<PreAuthorizationStatus>
    {
        [JsonConstructor]
        public RefreshPreAuthorizationStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
