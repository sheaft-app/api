using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateConsumerCommand : ConsumerCommand<bool>
    {
        [JsonConstructor]
        public UpdateConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
