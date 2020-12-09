using Sheaft.Domain.Enums;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class RefreshLegalValidationCommand : Command<bool>
    {
        [JsonConstructor]
        public RefreshLegalValidationCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
