using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class AcceptPurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public AcceptPurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool SkipNotification { get; set; }
    }
}