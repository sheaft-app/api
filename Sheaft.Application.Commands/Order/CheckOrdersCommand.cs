using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CheckOrdersCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckOrdersCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }
}
