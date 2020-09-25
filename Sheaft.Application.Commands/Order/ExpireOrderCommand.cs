using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ExpireOrderCommand : Command<bool>
    {
        [JsonConstructor]
        public ExpireOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
