using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CompletePurchaseOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public CompletePurchaseOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool SkipNotification { get; set; }
    }
}