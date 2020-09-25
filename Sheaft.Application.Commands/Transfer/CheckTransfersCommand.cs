using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckTransfersCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckTransfersCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
