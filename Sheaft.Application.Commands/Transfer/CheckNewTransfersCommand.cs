using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CheckNewTransfersCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckNewTransfersCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
