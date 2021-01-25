using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class GenerateProducersFileCommand : Command<bool>
    {
        [JsonConstructor]
        public GenerateProducersFileCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
