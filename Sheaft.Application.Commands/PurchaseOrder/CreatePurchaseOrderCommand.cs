using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreatePurchaseOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
        public Guid ProducerId { get; set; }
        public bool SkipNotification { get; set; }
    }
}
