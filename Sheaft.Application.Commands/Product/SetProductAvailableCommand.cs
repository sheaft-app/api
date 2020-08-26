using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class SetProductsAvailabilityCommand : Command<bool>
    {
        [JsonConstructor]
        public SetProductsAvailabilityCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public bool Available { get; set; }
    }
}
