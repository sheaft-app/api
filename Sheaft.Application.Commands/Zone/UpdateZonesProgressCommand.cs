using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateZonesProgressCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateZonesProgressCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
