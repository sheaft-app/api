using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class UnblockOrderCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-unblock-order";

        [JsonConstructor]
        public UnblockOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
