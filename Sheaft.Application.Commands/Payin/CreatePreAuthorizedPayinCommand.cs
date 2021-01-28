using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreatePreAuthorizedPayinCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePreAuthorizedPayinCommand(RequestUser requestUser) : base(requestUser)
        {
        }
        
        public Guid PreAuthorizationId { get; set; }
        public Guid PurchaseOrderId { get; set; }
    }
}
