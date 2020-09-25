using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class RefreshPayinStatusCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-refresh-payin-status";

        [JsonConstructor]
        public RefreshPayinStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
