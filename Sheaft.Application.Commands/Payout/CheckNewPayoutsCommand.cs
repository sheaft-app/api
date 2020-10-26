using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckNewPayoutsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckNewPayoutsCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
