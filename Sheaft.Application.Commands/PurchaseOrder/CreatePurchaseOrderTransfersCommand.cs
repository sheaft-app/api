using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreatePurchaseOrderTransfersCommand : Command<bool>
    {
        [JsonConstructor]
        public CreatePurchaseOrderTransfersCommand(RequestUser requestUser) : base(requestUser)
        {
        }
    }
}
