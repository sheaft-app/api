using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CheckNewTransferRefundsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckNewTransferRefundsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
