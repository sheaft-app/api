using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class GenerateZonesFileCommand : Command<bool>
    {
        [JsonConstructor]
        public GenerateZonesFileCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
