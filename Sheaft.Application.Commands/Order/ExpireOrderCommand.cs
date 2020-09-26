using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ExpireOrderCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-expire-order";

        [JsonConstructor]
        public ExpireOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
