using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class SetProductSearchabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProductSearchabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool VisibleToStores { get; set; }
        public bool VisibleToConsumers { get; set; }
    }
}
